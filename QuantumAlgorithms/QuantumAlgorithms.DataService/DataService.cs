using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using QuantumAlgorithms.Data;
using QuantumAlgorithms.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuantumAlgorithms.DataService
{
    public interface IDataService<TEntity>
        where TEntity : class, IEntity
    {
        (bool IsValid, string NotFoundParentId) AreRelationshipsValid(TEntity instance);
        EntityEntry Create(TEntity instance);
        void CreateMany(TEntity[] instances);
        EntityEntry Delete(TEntity instance);
        void DeleteMany(TEntity[] instances);
        bool Exists(Guid id);
        TEntity Get(Guid id);
        IQueryable<TEntity> GetMany();
        IQueryable<TEntity> GetManyFilter(Guid[] ids);
        int SaveChanges();
        EntityEntry Update(TEntity instance);
        void UpdateMany(TEntity[] instances);
    }

    public abstract class DataService<TEntity> : IDataService<TEntity>
        where TEntity : class, IEntity
    {
        protected QuantumAlgorithmsDbContext Context { get; }

        protected DataService(QuantumAlgorithmsDbContext context)
        {
            Context = context;
        }

        public (bool IsValid, string NotFoundParentId) AreRelationshipsValid(TEntity instance) => (true, null);
        public EntityEntry Create(TEntity instance) => Context.Add(instance);
        public void CreateMany(TEntity[] instances) => Context.AddRange(instances);
        public EntityEntry Delete(TEntity instance) => Context.Remove(instance);
        public void DeleteMany(TEntity[] instances) => Context.RemoveRange(instances);
        public bool Exists(Guid id) => Get(id) != null;
        public EntityEntry Update(TEntity instance) => Context.Update(instance);
        public void UpdateMany(TEntity[] instances) => Context.UpdateRange(instances);
        public int SaveChanges() => Context.SaveChanges();

        public abstract TEntity Get(Guid id);
        public abstract IQueryable<TEntity> GetMany();
        public abstract IQueryable<TEntity> GetManyFilter(Guid[] ids);

        protected string CombineFilterId(Guid[] ids) =>
            CombineFilter(ids.Select(id => id.ToString()), "Id");

        protected string CombineFilter(IEnumerable<string> data, string propertyName)
        {
            string combinedFilter = string.Empty;
            foreach (var value in data)
                combinedFilter = $"{combinedFilter} {propertyName} = '{value}' OR";
            return combinedFilter.Substring(0, combinedFilter.Length - 3);
        }
    }
}
