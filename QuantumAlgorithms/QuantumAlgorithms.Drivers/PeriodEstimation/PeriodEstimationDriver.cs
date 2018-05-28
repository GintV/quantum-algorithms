using System;
using System.Collections.Generic;
using Microsoft.Quantum.Simulation.Core;
using Microsoft.Quantum.Simulation.Simulators;
using QuantumAlgorithms.Common;

namespace QuantumAlgorithms.Drivers.PeriodEstimation
{
    public class PeriodEstimationDriver
    {
        public static QuantumSimulator QuantumSimulator;
        private IExecutionLogger _logger;

        public PeriodEstimationDriver(IExecutionLogger logger)
        {
            _logger = logger;
        }

        //public void SetLogger(IExecutionLogger logger) => _logger = logger;
        private IExecutionLogger Log => _logger;


        public long Run(int number, int modulus)
        {
            try
            {
                if (QuantumSimulator != null)
                    return PeriodEstimation.Run(QuantumSimulator, number, modulus).Result;

                Log.Error("Period estimation failed. Unexpected error occurred.");
                Log.Error("Fatal error. Aborting execution.");
                return -1;
            }
            catch (AggregateException e)
            {
                //Log.Warning("Period estimation failed.");

                foreach (Exception eInner in e.InnerExceptions)
                {
                    if (eInner is ExecutionFailException failException)
                    {
                        Log.Warning($"Period estimation failed. {failException.Message}");
                        //Console.WriteLine($"   {failException.Message}");
                    }
                    else
                    {
                        Log.Error("Period estimation failed. Unexpected error occurred.");
                        Log.Error("Fatal error. Aborting execution.");
                        return -1;
                    }
                }
            }

            return 0;
        }
    }
}
