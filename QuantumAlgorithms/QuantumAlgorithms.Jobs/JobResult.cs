namespace QuantumAlgorithms.Jobs
{
    public abstract class JobResult
    {
        public bool IsSuccess { get; }
        public bool HadWarnings { get; }

        protected JobResult(bool isSuccess, bool hadWarnings)
        {
            IsSuccess = isSuccess;
            HadWarnings = hadWarnings;
        }
    }
}