using System;

namespace QuantumAlgorithms.Drivers.PeriodEstimation
{
    public class PeriodEstimationDriverInput : IDriverInput
    {
        public Guid ExecutionId { get; set; }

        public int Number { get; set; }
        public int Modulus { get; set; }
    }
}
