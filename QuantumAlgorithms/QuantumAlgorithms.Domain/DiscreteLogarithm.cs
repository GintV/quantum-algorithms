using System;
using System.Collections.Generic;
using System.Text;

namespace QuantumAlgorithms.Domain
{
    public class DiscreteLogarithm : QuantumAlgorithm
    {
        // Table columns
        public int Generator { get; set; }
        public int Result { get; set; }
        public int Modulus { get; set; }
        public int Exponent { get; set; }
    }
}
