using System;
using QuantumAlgorithms.API.Extensions;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.API.Models.Get
{
    public class DiscreteLogarithmGetDto : IGetDto<DiscreteLogarithm, Guid>
    {
        public Guid Id { get; set; }

        public DiscreteLogarithmInputDto Input { get; set; }
        public DiscreteLogarithmOutputDto Output { get; set; }

        public Status Status { get; set; }
        public string StatusString => Status.GetStatusString();
        public ExecutionMessageDto[] Messages { get; set; }
    }
}
