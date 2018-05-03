using System;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.API.Models.Get
{
    public class ExecutionMessageDto
    {
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public ExecutionMessageSeverity Severity { get; set; }
        public string SeverityString => Severity.ToString();
    }
}