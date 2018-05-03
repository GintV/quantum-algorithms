using System;
using QuantumAlgorithms.API.Extensions;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.API.Models.Get
{
    public class IntegerFactorizationGetDto : IGetDto<IntegerFactorization, Guid>
    {
        public Guid Id { get; set; }

        public IntegerFactorizationInputDto Input { get; set; }
        public IntegerFactorizationOutputDto Output { get; set; }

        public Status Status { get; set; }
        public string StatusString => Status.GetStatusString();
        public ExecutionMessageDto[] Messages { get; set; }
    }
}
