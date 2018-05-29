using QuantumAlgorithms.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Hangfire;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Quantum.Simulation.Simulators;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;
using static QuantumAlgorithms.Jobs.Constants;
using Random = System.Random;
using QuantumAlgorithms.Drivers.PeriodEstimation;
using QuantumAlgorithms.DriversService;
using static QuantumAlgorithms.Common.Utilities;
using static QuantumAlgorithms.Jobs.DiscreteLogarithmJobResult;

namespace QuantumAlgorithms.Jobs
{
    public class DiscreteLogarithmJob : Job
    {
        private IExecutionLogger _logger;
        private readonly IDataService<DiscreteLogarithm> _dataService;
        private bool _hadWarnings;

        public DiscreteLogarithmJob(IExecutionLogger logger, IDataService<DiscreteLogarithm> dataService)
        {
            _logger = logger;
            _dataService = dataService;
            _hadWarnings = false;
        }

        public void SetLogger(IExecutionLogger logger) => _logger = logger;
        private IExecutionLogger Log => _logger;

        /// <summary>
        /// Runs discrete logarithm algorithm. 
        /// </summary>
        /// <param name="generator">Base of the logarithm.</param>
        /// <param name="result">Result of exponent modulus <paramref name="modulus"/>.</param>
        /// <param name="modulus">Modulus.</param>
        /// <returns>Factors discrete logarithm of base <paramref name="generator"/> modulus <paramref name="modulus"/>.</returns>
        public DiscreteLogarithmJobResult Run(int generator, int result, int modulus)
        {
            Log.Info($"Calculating discrete logarithm of g^(k) = r mod N | {generator}^(k) = {result} mod {modulus}.");
            if (result > modulus - 1)
            {
                Log.Error($"Remainder {result} is impossible to get after mod {modulus} operation. Aborting execution.");
                return FailResult();
            }

            if (result == generator)
            {
                Log.Info("Remainder is equal to generator, thus discrete logarithm is equal to 1.");
                return SuccessResult(1, _hadWarnings);
            }

            if (!generator.IsCoprime(modulus))
            {
                Log.Error("Generator is not a co-prime to modulus. Aborting execution.");
                return FailResult();
            }

            if (!result.IsCoprime(modulus))
            {
                Log.Error("Remainder is not a co-prime to modulus. Aborting execution.");
                return FailResult();
            }

            Log.Info($"Estimating multiple of period of a bivariate function (a, b) => g^(a) * r^(b) mod N " +
                $"| (a, b) => {generator}^(a) * {result}^(b) (mod {modulus}).");

            Log.Info($"Enqueuing period estimation of univariate function (a) => g^(a) mod N | (a) => {generator}^(a) mod {modulus}.");
            if (!TryEstimatePeriod(generator, modulus, out var generatorPeriod))
                return FailResult();
            Log.Info($"Estimated generator period: {generatorPeriod}");

            if (result == 1)
            {
                Log.Info($"Result is equal to 1, thus discrete logarithm is equal to generator period {generatorPeriod}.");
                return SuccessResult(generatorPeriod, _hadWarnings);
            }

            Log.Info($"Enqueuing period estimation of univariate function (b) => r^(b) mod N | (b) => {result}^(b) mod {modulus}.");
            if (!TryEstimatePeriod(result, modulus, out var resultPeriod))
                return FailResult();
            Log.Info($"Estimated result period: {resultPeriod}");

            Log.Info($"Estimated multiple of period of a bivariate function: ({generatorPeriod}, {resultPeriod}).");
            Log.Info($"Trying to reduce multiple of period by GCD of ({generatorPeriod}, {resultPeriod}) or at least by half.");
            var periodsGcd = GCD(generatorPeriod, resultPeriod);
            var basePeriod = generatorPeriod;

            if ((CalculateExpMod(generator, generatorPeriod / periodsGcd, modulus) *
                 CalculateExpMod(result, resultPeriod / periodsGcd, modulus)) % modulus == 1)
            {
                generatorPeriod = generatorPeriod / periodsGcd;
                resultPeriod = resultPeriod / periodsGcd;
                Log.Info($"Successfully reduced multiple of period to: ({generatorPeriod}, {resultPeriod}).");
            }
            else if (periodsGcd % 2 == 0)
            {
                generatorPeriod = generatorPeriod / 2;
                resultPeriod = resultPeriod / 2;
                Log.Info($"Successfully reduced multiple of period to: ({generatorPeriod}, {resultPeriod}).");
            }
            else
            {
                Log.Info("Could not reduce period.");
            }

            Log.Info($"Calculating multiple ({generatorPeriod}, {resultPeriod}) multiply of a period.");
            var exponentSum = basePeriod;
            var multiply = 1;
            for (; multiply < modulus / 2; ++multiply)
            {
                var discreteLogarithm = (exponentSum - generatorPeriod) / resultPeriod;
                if (CalculateExpMod(generator, discreteLogarithm, modulus) == result)
                {
                    Log.Info($"Calculated multiply: {multiply}.");
                    Log.Info($"Using equation a + b*k = a * m | {generatorPeriod} + {resultPeriod}*k = {generatorPeriod} * {multiply}, " +
                        $"where m is calculated multiply, to determine discrete logarithm, thus it is equal to {discreteLogarithm}.");
                    return SuccessResult(discreteLogarithm, _hadWarnings);
                }

                exponentSum += basePeriod;
            }

            Log.Error($"Failed to calculate multiple ({generatorPeriod}, {resultPeriod}) multiply of a period. Multiply is greater than {multiply} and " +
                $"thus it is inefficient to calculate discrete logarithm, or generator {generator} is not the actual generator of " +
                $"modulus {modulus} and remainder {result} could actually be impossible to get.");

            return FailResult();
        }

        private bool TryEstimatePeriod(int a, int number, out int period)
        {
            var failCount = 0;
            while (!TryEstimatePeriodHelper(a, number, out period))
            {
                _hadWarnings = true;
                ++failCount;
                if (failCount > 2)
                {
                    Log.Error("Period estimation failed 3 times in a row. Aborting execution.");
                    return false;
                }
                if (period == -1)
                    return false;
                Log.Info("Enqueuing period estimation retry.");
            }
            return true;
        }

        private bool TryEstimatePeriodHelper(int a, int number, out int period)
        {
            lock (Lock)
            {
                try
                {
                    var entity = _dataService.Get(_logger.GetExecutionId());
                    if (entity == null || entity.Status == Status.Canceled || entity.Messages.Last().Message.Contains("canceled"))
                    {
                        period = -1;
                        return false;
                    }

                    PeriodEstimationDriver.QuantumSimulator = new QuantumSimulator(false, 5);
                    Log.Info("Period estimation started.");
                    var jobId = BackgroundJob.Enqueue<IDriverService<PeriodEstimationDriverInput, int>>(driverService =>
                        driverService.Run(new PeriodEstimationDriverInput
                        {
                            ExecutionId = _logger.GetExecutionId(),
                            Number = a,
                            Modulus = number
                        }));

                    entity = _dataService.Get(_logger.GetExecutionId());
                    entity.InnerJobId = jobId;
                    _dataService.Update(entity);
                    _dataService.SaveChanges();

                    period = WaitPeriodEstimation(jobId);
                }
                catch (TimeoutException e)
                {
                    Log.Error($"Period estimation failed. {e.Message} Aborting execution.");
                    period = -1;
                }
                catch (OperationCanceledException)
                {
                    period = -1;
                }
                catch (Exception e) when (e is KeyNotFoundException || e is FormatException)
                {
                    Log.Error($"Period estimation failed. {e.Message}");
                    Log.Error("Fatal error. Aborting execution.");
                    period = -1;
                }
            }

            return period >= 1;
        }

        private int WaitPeriodEstimation(string jobId)
        {
            var connection = JobStorage.Current.GetConnection();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (connection.GetJobData(jobId).State != SucceededState.StateName)
            {
                Thread.Sleep(5000);
                if (stopwatch.ElapsedMilliseconds > PeriodEstimationTimeoutMilliseconds)
                {
                    PeriodEstimationDriver.QuantumSimulator?.Dispose();
                    PeriodEstimationDriver.QuantumSimulator = null;
                    BackgroundJob.Delete(jobId);
                    throw new TimeoutException("Period estimation reached a timeout.");
                }
                var entity = _dataService.Get(_logger.GetExecutionId());
                if (entity.CancelJob)
                {
                    PeriodEstimationDriver.QuantumSimulator?.Dispose();
                    PeriodEstimationDriver.QuantumSimulator = null;
                    BackgroundJob.Delete(jobId);
                    throw new OperationCanceledException();
                }
            }

            PeriodEstimationDriver.QuantumSimulator?.Dispose();
            PeriodEstimationDriver.QuantumSimulator = null;

            if (!connection.GetStateData(jobId).Data.TryGetValue("Result", out var result))
                throw new KeyNotFoundException("Could not receive result from period estimation job.");

            if (!int.TryParse(result, out var period))
                throw new FormatException("Could not get period value from period estimation result.");

            return period;
        }
    }
}