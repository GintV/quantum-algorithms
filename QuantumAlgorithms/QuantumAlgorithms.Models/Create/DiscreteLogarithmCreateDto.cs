using System;
using System.ComponentModel.DataAnnotations;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.Models.Create
{
    public class DiscreteLogarithmCreateDto : ICreateDto<DiscreteLogarithm>
    {
        [Required, Range(1, int.MaxValue)]
        public int Result { get; set; }
        [Required, Range(2, int.MaxValue)]
        public int Generator { get; set; }
        [Required, Range(2, int.MaxValue)]
        public int Modulus { get; set; }

        public DateTime StartTime { get; set; } = DateTime.Now;
    }
}
