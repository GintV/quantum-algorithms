using System.Linq;
using QuantumAlgorithms.API.QueryingParameters;
using QuantumAlgorithms.Domain;
using QuantumAlgorithms.DataService;

namespace QuantumAlgorithms.API.Extensions
{
    public static class DataServiceExtensions
    {
        public static IQueryable<TEntity> GetMany<TEntity, TIdentifier>(this IDataService<TEntity, TIdentifier> dataService,
            BaseResourceParameters baseResourceParameters) where TEntity : class, IEntity =>
            dataService.GetMany().ApplyPaging(baseResourceParameters);

        public static IQueryable<TEntity> GetManyFilter<TEntity, TIdentifier>(this IDataService<TEntity, TIdentifier> dataService,
            BaseResourceParameters baseResourceParameters, FilterByIdsParameters<TIdentifier> filterByIdsParameters)
            where TEntity : class, IEntity => (filterByIdsParameters?.GetIds()).Any() ? 
            dataService.GetManyFilter(filterByIdsParameters.GetIds().ToArray()).ApplyPaging(baseResourceParameters) :
            dataService.GetMany(baseResourceParameters);

        private static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> source, BaseResourceParameters baseResourceParameters) =>
            source.Skip(baseResourceParameters.PageSize * (baseResourceParameters.Page - 1)).Take(baseResourceParameters.PageSize);
    }
}
