using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using QuantumAlgorithms.Models.Create;
using QuantumAlgorithms.Models.Get;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.API.Extensions
{
    public static class MapperConfigurationExpressionExtensions
    {
        public static void SetupAutoMapper(this IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<IntegerFactorization, IntegerFactorizationGetDto>().
                ForMember(destination => destination.Input, options =>
                    options.MapFrom(src => IntegerFactorizationInputDto.Create(src.Number))).
                ForMember(destination => destination.Output, options =>
                    options.MapFrom(src => src.FactorP == 0 ? null : IntegerFactorizationOutputDto.Create(src.FactorP, src.FactorQ)));
            configuration.CreateMap<IntegerFactorizationCreateDto, IntegerFactorization>();

            configuration.CreateMap<DiscreteLogarithm, DiscreteLogarithmGetDto>().
                ForMember(destination => destination.Input, options =>
                    options.MapFrom(src => DiscreteLogarithmInputDto.Create(src.Generator, src.Result, src.Modulus))).
                ForMember(destination => destination.Output, options =>
                    options.MapFrom(src => src.Exponent == 0 ? null : DiscreteLogarithmOutputDto.Create(src.Exponent)));
            configuration.CreateMap<DiscreteLogarithmCreateDto, DiscreteLogarithm>();

            configuration.CreateMap<ExecutionMessage, ExecutionMessageDto>();
        }
    }
}
