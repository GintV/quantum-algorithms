using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.Models
{
    public static class StatusExtensions
    {
        public static string GetStatusString(this Status status)
        {
            switch (status)
            {
                case Status.Enqueued: return "Enqueued"; 
                case Status.InProgress: return "In Progress";
                case Status.Finished: return "Finished";
                case Status.FinishedWithWarnings: return "Finished With Warnings";
                case Status.FinishedWithErrors: return "Finished With Errors";
                case Status.Canceled: return "Canceled";
                default: return "Unknown";
            }
        }
    }
}