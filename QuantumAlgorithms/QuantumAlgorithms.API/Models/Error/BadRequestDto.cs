using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumAlgorithms.API.Models.Error
{
    public class BadRequestDto : IErrorDto
    {
        public string Error => "400 Bad Request";
        public string ErrorDescription { get; set; }

        public static IErrorDto InvalidData() => new BadRequestDto { ErrorDescription = "Model contains fields with invalid values." };
    }
}
