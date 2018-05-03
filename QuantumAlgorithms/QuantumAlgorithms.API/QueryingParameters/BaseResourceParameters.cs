using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumAlgorithms.API.QueryingParameters
{
    public class BaseResourceParameters
    {
        private const int MaxPageSize = 20;
        private const int DefaultPageSize = 10;

        private int _pageSize = DefaultPageSize;

        public int Page { get; set; } = 1;
        public int PageSize { get => _pageSize; set => _pageSize = value > MaxPageSize ? MaxPageSize : value; }
    }
}
