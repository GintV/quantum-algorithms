using System;
using System.Collections.Generic;
using System.Text;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.Models.Get
{
    public class QuantumAlgorithmGetDto
    {
        public Status Status { get; set; }
        public string StatusString => Status.GetStatusString();
        public DateTime StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public ExecutionMessageDto[] Messages { get; set; }
    }
}
