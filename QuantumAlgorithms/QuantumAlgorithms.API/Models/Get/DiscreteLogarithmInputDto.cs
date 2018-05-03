using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumAlgorithms.API.Models.Get
{
    public class DiscreteLogarithmInputDto
    {
        public int Generator { get; set; }
        public int Result { get; set; }
        public int Modulus { get; set; }

        public static DiscreteLogarithmInputDto Create(int generator, int result, int modulus) =>
            new DiscreteLogarithmInputDto {Generator = generator, Result = result, Modulus = modulus};
    }
}
