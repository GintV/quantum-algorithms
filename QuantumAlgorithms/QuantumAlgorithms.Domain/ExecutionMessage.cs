using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantumAlgorithms.Domain
{
    public class ExecutionMessage : IEntity
    {
        // Table columns
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public ExecutionMessageSeverity Severity { get; set; }
        public Guid QuantumAlgorithmId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(QuantumAlgorithmId))]
        public QuantumAlgorithm QuantumAlgorithm { get; set; }
    }

    public enum ExecutionMessageSeverity
    {
        Info,
        Warning,
        Error
    }
}