using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuantumAlgorithms.Domain
{
    public class QuantumAlgorithm : IEntity
    {
        // Table columns
        public Guid Id { get; set; }
        public Status Status { get; set; }

        // Navigation properties
        [InverseProperty(nameof(ExecutionMessage.QuantumAlgorithm))]
        public ICollection<ExecutionMessage> Messages { get; set; }
    }

    public enum Status
    {
        InProgress,
        Finished,
        FinishedWithWarnings,
        FinishedWithErrors
    }
}
