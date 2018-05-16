using System;
using Hangfire;
using QuantumAlgorithms.Common;
using QuantumAlgorithms.Drivers.PeriodEstimation;

namespace QuantumAlgorithms.DriversService
{
    public class PeriodEstimationDriverService : DriverService<PeriodEstimationDriverInput, int>
    {
        public PeriodEstimationDriverService(IExecutionLogger logger) : base(logger) { }

        public override int Run(PeriodEstimationDriverInput driverInput)
        {
            Logger.SetExecutionId(driverInput.ExecutionId);
            var driver = new PeriodEstimationDriver(Logger);
            return (int)driver.Run(driverInput.Number, driverInput.Modulus);
        }
    }
}
