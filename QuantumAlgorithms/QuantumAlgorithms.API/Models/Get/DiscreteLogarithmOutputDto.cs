using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumAlgorithms.API.Models.Get
{
    public class DiscreteLogarithmOutputDto
    {
        public int DiscreteLogarithm { get; set; }

        public static DiscreteLogarithmOutputDto Create(int discreteLogarithm) =>
            new DiscreteLogarithmOutputDto {DiscreteLogarithm = discreteLogarithm};
    }
}
