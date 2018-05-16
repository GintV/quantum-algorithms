using System;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.Models.Get
{
    public class IntegerFactorizationGetDto : QuantumAlgorithmGetDto, IGetDto<IntegerFactorization>
    {
        public Guid Id { get; set; }

        public IntegerFactorizationInputDto Input { get; set; }
        public IntegerFactorizationOutputDto Output { get; set; }

    }
}
