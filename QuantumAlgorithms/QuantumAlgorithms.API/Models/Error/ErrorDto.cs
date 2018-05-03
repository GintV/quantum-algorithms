using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumAlgorithms.API.Models.Error
{
    public interface IErrorDto
    {
        string Error { get; }
        string ErrorDescription { get; }
    }
}
