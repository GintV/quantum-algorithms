namespace QuantumAlgorithms.Jobs
{
    public class IntegerFactorizationJobResult : JobResult
    {
        public (int P, int Q) Factors { get; }

        private IntegerFactorizationJobResult(bool isSuccess, bool hadWarnings, (int, int) factors) : base(isSuccess, hadWarnings)
        {
            Factors = factors;
        }

        public static IntegerFactorizationJobResult SuccessResult((int, int) factors) => new IntegerFactorizationJobResult(true, false, factors);

        public static IntegerFactorizationJobResult WarningResult((int, int) factors) => new IntegerFactorizationJobResult(true, true, factors);

        public static IntegerFactorizationJobResult FailResult() => new IntegerFactorizationJobResult(false, false, (0, 0));
    }
}