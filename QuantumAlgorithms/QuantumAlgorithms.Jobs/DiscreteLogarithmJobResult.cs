namespace QuantumAlgorithms.Jobs
{
    public class DiscreteLogarithmJobResult : JobResult
    {
        public int DiscreteLogarithm { get; }

        private DiscreteLogarithmJobResult(bool isSuccess, bool hadWarnings, int discreteLogarithm) : base(isSuccess, hadWarnings)
        {
            DiscreteLogarithm = discreteLogarithm;
        }

        public static DiscreteLogarithmJobResult SuccessResult(int discreteLogarithm, bool hadWarnings) =>
            new DiscreteLogarithmJobResult(true, hadWarnings, discreteLogarithm);

        public static DiscreteLogarithmJobResult FailResult() => new DiscreteLogarithmJobResult(false, false, 0);
    }
}