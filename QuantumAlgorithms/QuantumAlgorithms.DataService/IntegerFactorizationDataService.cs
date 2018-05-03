using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using QuantumAlgorithms.Data;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.DataService
{
    public class IntegerFactorizationDataService : DataService<IntegerFactorization, Guid>
    {
        public IntegerFactorizationDataService(QuantumAlgorithmsDbContext context) : base(context) { }

        public override IntegerFactorization Get(Guid id) => Context.IntegerFactorizationRuns.Include(run => run.Messages).
            FirstOrDefault(run => run.Id == id);
        public override IQueryable<IntegerFactorization> GetMany() => Context.IntegerFactorizationRuns.Include(run => run.Messages);
        public override IQueryable<IntegerFactorization> GetManyFilter(Guid[] ids) => Context.IntegerFactorizationRuns.
            FromSql($"SELECT * FROM QuantumAlgorithm WHERE Discriminator = 'IntegerFactorization' AND ({CombineFilter(ids)})".ToString()).
            Include(run => run.Messages);
    }
}
