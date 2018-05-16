using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Quantum.Primitive;

namespace QuantumAlgorithms.API.QueryingParameters
{
    public class FilterByIdsParameter
    {
        public string Ids { get; set; }

        public IEnumerable<Guid> GetIds()
        {
            if (Ids == null || !Ids.Any() || Ids.First() != '[' || Ids.Last() != ']')
                yield break;

            foreach (var id in new string(Ids.Skip(1).Take(Ids.Length - 2).ToArray()).Split(','))
            {
                var trimmedId = id.Trim();
                if (Guid.TryParse(trimmedId, out var result))
                    yield return result;
            }
        }
    }
}
