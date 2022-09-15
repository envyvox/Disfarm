using System;
using AutoMapper;
using Disfarm.Data.Enums;

namespace Disfarm.Services.Game.Seed.Models
{
    public record SeedDto(
        Guid Id,
        string Name,
        Season Season,
        uint GrowthDays,
        uint ReGrowthDays,
        bool IsMultiply,
        uint Price,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);

    public class SeedToDtoProfile : Profile
    {
        public SeedToDtoProfile() => CreateMap<Data.Entities.Seed, SeedDto>().MaxDepth(2);
    }

    public class DtoToSeedProfile : Profile
    {
        public DtoToSeedProfile() => CreateMap<SeedDto, Data.Entities.Seed>().MaxDepth(2);
    }
}