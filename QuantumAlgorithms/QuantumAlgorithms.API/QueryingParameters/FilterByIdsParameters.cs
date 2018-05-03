using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumAlgorithms.API.QueryingParameters
{
    public class FilterByIdsParameters<TIdentifier>
    {
        public TIdentifier[] Ids { get; set; }
    }
}
