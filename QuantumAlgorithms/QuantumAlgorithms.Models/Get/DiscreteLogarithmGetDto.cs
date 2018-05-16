using System;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.Models.Get
{
    public class DiscreteLogarithmGetDto : QuantumAlgorithmGetDto, IGetDto<DiscreteLogarithm>
    {
        public Guid Id { get; set; }

        public DiscreteLogarithmInputDto Input { get; set; }
        public DiscreteLogarithmOutputDto Output { get; set; }

    }
}
