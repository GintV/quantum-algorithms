using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Quantum.Primitive;

namespace QuantumAlgorithms.API.QueryingParameters
{
    public class FilterByStatusesParameter
    {
        public string Statuses { get; set; }

        public IEnumerable<int> GetStatuses()
        {
            if (int.TryParse(Statuses ?? string.Empty, out var status))
            {
                yield return status;
                yield break;
            }

            if (Statuses == null || !Statuses.Any() || Statuses.First() != '[' || Statuses.Last() != ']')
                yield break;

            foreach (var id in new string(Statuses.Skip(1).Take(Statuses.Length - 2).ToArray()).Split(','))
            {
                var trimmedId = id.Trim();
                if (int.TryParse(trimmedId, out var result))
                    yield return result;
            }
        }
    }
}
