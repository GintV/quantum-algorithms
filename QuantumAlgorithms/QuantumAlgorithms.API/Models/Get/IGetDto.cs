namespace QuantumAlgorithms.API.Models.Get
{
    public interface IGetDto<TEntity, TIdentifier>
    {
        TIdentifier Id { get; }
    }
}
