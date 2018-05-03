using System;
using Microsoft.EntityFrameworkCore;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.Data
{
    public class QuantumAlgorithmsDbContext : DbContext
    {
        public DbSet<DiscreteLogarithm> DiscreteLogarithmRuns { get; set; }
        public DbSet<ExecutionMessage> ExecutionMessages { get; set; }
        public DbSet<IntegerFactorization> IntegerFactorizationRuns { get; set; }

        public QuantumAlgorithmsDbContext(DbContextOptions options) : base(options) { }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<GroupSprint>().HasKey(entity => new { entity.GroupId, entity.SprintId });
        //    modelBuilder.Entity<PlayerSprint>().HasKey(entity => new { entity.PlayerId, entity.SprintId });
        //    modelBuilder.Entity<PlayerSubscription>().HasKey(entity => new { entity.PlayerId, entity.GroupId });
        //}
    }
}
