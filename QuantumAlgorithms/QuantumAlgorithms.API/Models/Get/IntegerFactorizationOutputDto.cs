using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumAlgorithms.API.Models.Get
{
    public class IntegerFactorizationOutputDto
    {
        public int P { get; set; }
        public int Q { get; set; }

        public static IntegerFactorizationOutputDto Create(int p, int q) => new IntegerFactorizationOutputDto { P = p, Q = q };
    }

    //public class Factors
    //{
    //    public int P { get; set; }
    //    public int Q { get; set; }

    //    public Factors(int p, int q)
    //    {
    //        P = p;
    //        Q = q;
    //    }
    //}
}
