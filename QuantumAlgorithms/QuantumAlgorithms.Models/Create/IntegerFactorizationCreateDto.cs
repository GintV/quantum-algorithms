﻿using System;
using System.ComponentModel.DataAnnotations;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.Models.Create
{
    public class IntegerFactorizationCreateDto : ICreateDto<IntegerFactorization>
    {
        [Required, Range(2, int.MaxValue)]
        public int Number { get; set; }

        public DateTime StartTime { get; set; } = DateTime.Now;
    }

    //public class CityParentableCreateDto : IParentableCreateDto<City, Guid>
    //{
    //    [Required]
    //    public string Title { get; set; }
    //    public Guid? CountryId { get; set; }

    //    // Interface realization
    //    public void SetParentId(Guid parentId) => CountryId = parentId;
    //}
}
