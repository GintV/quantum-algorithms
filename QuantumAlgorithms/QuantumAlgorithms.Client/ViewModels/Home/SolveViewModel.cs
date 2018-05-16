using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumAlgorithms.Client.ViewModels.Home
{
    public class SolveViewModel
    {
        public Problem Problem { get; set; }
        [Required, Range(2, 100)]
        public int Number { get; set; }
        [Required, Range(2, 100)]
        public int Generator { get; set; }
        [Required, Range(2, 100)]
        public int Modulus { get; set; }
        [Required, Range(2, 100)]
        public int Result { get; set; }

        public bool IsValid { get; set; } = true;
        public bool ApiRequestFailed { get; set; } = false;
    }

    public enum Problem
    {
        IntegerFactorization = 0,
        DiscreteLogarithm = 1
    }
}
