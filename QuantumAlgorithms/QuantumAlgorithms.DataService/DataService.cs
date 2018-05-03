using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using QuantumAlgorithms.Data;
using QuantumAlgorithms.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuantumAlgorithms.DataService
{
    public interface IDataService<TEntity, TIdentifier>
        where TEntity : class, IEntity
    {
        (bool IsValid, string NotFoundParentId) AreRelationshipsValid(TEntity instance);
        EntityEntry Create(TEntity instance);
        void CreateMany(TEntity[] instances);
        EntityEntry Delete(TEntity instance);
        void DeleteMany(TEntity[] instances);
        bool Exists(TIdentifier id);
        TEntity Get(TIdentifier id);
        IQueryable<TEntity> GetMany();
        IQueryable<TEntity> GetManyFilter(Guid[] ids);
        int SaveChanges();
        EntityEntry Update(TEntity instance);
        void UpdateMany(TEntity[] instances);
    }

    public abstract class DataService<TEntity, TIdentifier> : IDataService<TEntity, TIdentifier>
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
        public bool Exists(TIdentifier id) => Get(id) != null;
        public EntityEntry Update(TEntity instance) => Context.Update(instance);
        public void UpdateMany(TEntity[] instances) => Context.UpdateRange(instances);
        public int SaveChanges() => Context.SaveChanges();

        public abstract TEntity Get(TIdentifier id);
        public abstract IQueryable<TEntity> GetMany();
        public abstract IQueryable<TEntity> GetManyFilter(Guid[] ids);

        protected string CombineFilter(TIdentifier[] ids)
        {
            string combinedFilter = string.Empty;
            foreach (var id in ids)
                combinedFilter = $"{combinedFilter} Id = '{id}' OR";
            return combinedFilter.Substring(0, combinedFilter.Length - 3);
        }
    }
}
