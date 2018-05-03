using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumAlgorithms.API.Models.Get
{
    public class IntegerFactorizationInputDto
    {
        public int Number { get; set; }

        public static IntegerFactorizationInputDto Create(int number) => new IntegerFactorizationInputDto { Number = number };
    }
}
