using QuantumAlgorithms.Common;
using QuantumAlgorithms.Drivers;

namespace QuantumAlgorithms.DriversService
{
    public interface IDriverService<TDriverInput, TResult>
        where TDriverInput : IDriverInput
    {
        TResult Run(TDriverInput driverInput);
    }
    public abstract class DriverService<TDriverInput, TResult> : IDriverService<TDriverInput, TResult>
        where TDriverInput : IDriverInput
    {
        protected IExecutionLogger Logger { get; }

        protected DriverService(IExecutionLogger logger)
        {
            Logger = logger;
        }

        public abstract TResult Run(TDriverInput driverInput);
    }
}
