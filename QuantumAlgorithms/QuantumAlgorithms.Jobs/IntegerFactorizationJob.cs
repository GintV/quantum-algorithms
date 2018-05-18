using QuantumAlgorithms.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Hangfire;
using Hangfire.States;
using Microsoft.Quantum.Simulation.Simulators;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;
using static QuantumAlgorithms.Jobs.Constants;
using Random = System.Random;
using QuantumAlgorithms.Drivers.PeriodEstimation;
using QuantumAlgorithms.DriversService;
using static QuantumAlgorithms.Jobs.IntegerFactorizationJobResult;

namespace QuantumAlgorithms.Jobs
{
    public class IntegerFactorizationJob : Job
    {
        private IExecutionLogger _logger;
        private readonly IDataService<IntegerFactorization> _dataService;
        private bool _hadWarnings;

        public IntegerFactorizationJob(IExecutionLogger logger, IDataService<IntegerFactorization> dataService)
        {
            _logger = logger;
            _dataService = dataService;
            _hadWarnings = false;
        }

        public void SetLogger(IExecutionLogger logger) => _logger = logger;
        private IExecutionLogger Log => _logger;

        /// <summary>
        /// Runs integer factorization algorithm.
        /// </summary>
        /// <param name="number">Number to factor.</param>
        /// <returns>Factors of the <paramref name="number"/>.</returns>
        public IntegerFactorizationJobResult Run(int number)
        {
            Log.Info($"Factoring {number}.");
            if (number.IsEven())
            {
                Log.Info("Provided number is even.");
                Log.Info($"Factors are: 2 and {number / 2}");
                return SuccessResult((2, number / 2));
            }

            var retryCount = 0;
            for (; retryCount < 3; ++retryCount)
            {
                Log.Info($"Trying to pick a co-prime to provided number in the interval [2; number - 1] | [2, {number - 1}].");
                if (!TryPickCoprime(number, out int coprime, out int divisor))
                {
                    Log.Info("Found divisor by accident.");
                    Log.Info($"Factors are: {divisor} and {number / divisor}");
                    return SuccessResult((number / divisor, divisor));
                }

                Log.Info($"No common divisors found. Enqueuing period estimation of univariate function (x) => a^(x) mod N " +
                    $"| (x) => {coprime}^(x) (mod {number}).");
                if (!TryEstimatePeriod(coprime, number, out var period))
                    break;

                Log.Info($"Estimated period: {period}.");
                if (period % 2 != 0)
                {
                    _hadWarnings = true;
                    Log.Warning("Period is odd, thus we have to pick another number to estimate period of and start over.");
                    continue;
                }

                Log.Info($"Checking equality (a^(r/2) + 1) mod N = 0 | ({coprime}^({period / 2}) + 1) mod {number} = 0.");
                var halfPower = Utilities.CalculateExpMod(coprime, period / 2, number);

                if (halfPower == number - 1)
                {
                    _hadWarnings = true;
                    Log.Warning("Equality is correct. It's a trivial case, thus we have to pick another number to estimate period of and start over.");
                    continue;
                }
                Log.Info($"Equality is incorrect, thus p*q = N, where p = GCD(a^(r/2) - 1, N) and q = GCD(a^(r/2) + 1, N) " +
                    $"| p = GCD({coprime}^({period / 2}) - 1, {number}) and q = GCD({coprime}^({period / 2}) + 1, {number}).");

                var p = Utilities.GCD(halfPower - 1, number);
                var q = Utilities.GCD(halfPower + 1, number);

                Log.Info($"Factors are: {p} and {q}.");
                return _hadWarnings ? WarningResult((p, q)) : SuccessResult((p, q));
            }

            if (retryCount > 2)
                Log.Error("Algorithm yielded no result after 3 attempts. Aborting execution.");

            return FailResult();
        }

        private bool TryPickCoprime(int number, out int coprime, out int divisor)
        {
            var random = new Random();
            coprime = random.Next(number - 2) + 2;
            Log.Info($"Picked {coprime} as co-prime candidate. Checking if {coprime} has any common divisors with {number}.");
            divisor = Utilities.GCD(coprime, number);
            return divisor == 1;
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
            try
            {
                lock (Lock)
                {
                    Log.Info("Period estimation started.");
                    PeriodEstimationDriver.QuantumSimulator = new QuantumSimulator();
                    var jobId = BackgroundJob.Enqueue<IDriverService<PeriodEstimationDriverInput, int>>(driverService =>
                        driverService.Run(new PeriodEstimationDriverInput
                        {
                            ExecutionId = _logger.GetExecutionId(),
                            Number = a,
                            Modulus = number
                        }));

                    var entity = _dataService.Get(_logger.GetExecutionId());
                    entity.InnerJobId = jobId;
                    _dataService.Update(entity);
                    _dataService.SaveChanges();

                    period = WaitPeriodEstimation(jobId);
                }
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
                if (entity.Messages.Last().Severity == ExecutionMessageSeverity.Error)
                {
                    PeriodEstimationDriver.QuantumSimulator?.Dispose();
                    PeriodEstimationDriver.QuantumSimulator = null;
                    BackgroundJob.Delete(jobId);
                    throw new OperationCanceledException();
                }
            }

            if (!connection.GetStateData(jobId).Data.TryGetValue("Result", out var result))
                throw new KeyNotFoundException("Could not receive result from period estimation job.");

            if (!int.TryParse(result, out var period))
                throw new FormatException("Could not get period value from period estimation result.");

            return period;
        }
    }
}