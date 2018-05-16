using System.Linq;
using QuantumAlgorithms.API.QueryingParameters;
using QuantumAlgorithms.Domain;
using QuantumAlgorithms.DataService;

namespace QuantumAlgorithms.API.Extensions
{
    public static class DataServiceExtensions
    {
        //public static IQueryable<TEntity> GetMany<TEntity, TIdentifier>(this IDataService<TEntity, TIdentifier> dataService,
        //    BaseResourceParameters baseResourceParameters) where TEntity : class, IEntity =>
        //    dataService.GetMany().ApplyPaging(baseResourceParameters);

        public static IQueryable<TEntity> GetManyFilter<TEntity>(this IDataService<TEntity> dataService,
            BaseResourceParameters baseResourceParameters, FilterByIdsParameter filterByIdsParameter,
            FilterByStatusesParameter filterByStatusesParameter, string subscriberId, bool applyPaging = true) where TEntity : QuantumAlgorithm =>
            ((filterByIdsParameter?.GetIds()).Any() ? dataService.GetManyFilter(filterByIdsParameter.GetIds().ToArray()) : dataService.GetMany()).
            Where(entity => entity.SubscriberId == subscriberId).Where(entity => filterByStatusesParameter == null ||
            !filterByStatusesParameter.GetStatuses().Any() || filterByStatusesParameter.GetStatuses().Contains((int)entity.Status)).
            MaybeApplyPaging(baseResourceParameters, applyPaging);

        private static IQueryable<TEntity> MaybeApplyPaging<TEntity>(this IQueryable<TEntity> source, BaseResourceParameters baseResourceParameters,
            bool applyPaging) => applyPaging ? source.ApplyPaging(baseResourceParameters) : source;

        public static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> source, BaseResourceParameters baseResourceParameters) =>
            source.Skip(baseResourceParameters.PageSize * (baseResourceParameters.Page - 1)).Take(baseResourceParameters.PageSize);
    }
}
