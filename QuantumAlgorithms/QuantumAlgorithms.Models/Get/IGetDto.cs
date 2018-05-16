using System;

namespace QuantumAlgorithms.Models.Get
{
    public interface IGetDto<TEntity>
    {
        Guid Id { get; }
    }
}
