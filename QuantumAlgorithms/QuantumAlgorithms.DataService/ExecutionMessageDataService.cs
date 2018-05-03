using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using QuantumAlgorithms.Data;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.DataService
{
    public class ExecutionMessageDataService : DataService<ExecutionMessage, Guid>
    {
        public ExecutionMessageDataService(QuantumAlgorithmsDbContext context) : base(context) { }

        public override ExecutionMessage Get(Guid id) => null;
        public override IQueryable<ExecutionMessage> GetMany() => Enumerable.Empty<ExecutionMessage>().AsQueryable();
        public override IQueryable<ExecutionMessage> GetManyFilter(Guid[] ids) => Enumerable.Empty<ExecutionMessage>().AsQueryable();
    }
}
