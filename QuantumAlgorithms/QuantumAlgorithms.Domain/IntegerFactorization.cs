using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantumAlgorithms.Domain
{
    public class IntegerFactorization : QuantumAlgorithm
    {
        // Table columns
        public int Number { get; set; }
        public int FactorP { get; set; }
        public int FactorQ { get; set; }

    }
}
