using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using QuantumAlgorithms.Data;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.DataService
{
    public class DiscreteLogarithmDataService : DataService<DiscreteLogarithm, Guid>
    {
        public DiscreteLogarithmDataService(QuantumAlgorithmsDbContext context) : base(context) { }

        public override DiscreteLogarithm Get(Guid id) => Context.DiscreteLogarithmRuns.Include(run => run.Messages).
            FirstOrDefault(run => run.Id == id);
        public override IQueryable<DiscreteLogarithm> GetMany() => Context.DiscreteLogarithmRuns.Include(run => run.Messages);
        public override IQueryable<DiscreteLogarithm> GetManyFilter(Guid[] ids) => Context.DiscreteLogarithmRuns.
            FromSql($"SELECT * FROM DiscreteLogarithmRuns WHERE {CombineFilter(ids)}".ToString());
    }
}
