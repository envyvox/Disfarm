using System;
using AutoMapper;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.Crop.Models;

namespace Disfarm.Services.Game.Seed.Models
{
    public record SeedDto(
        Guid Id,
        string Name,
        Season Season,
        TimeSpan Growth,
        TimeSpan? ReGrowth,
        bool IsMultiply,
        uint Price,
        CropDto Crop,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);

    public class SeedToDtoProfile : Profile
    {
        public SeedToDtoProfile() => CreateMap<Data.Entities.Resource.Seed, SeedDto>().MaxDepth(3);
    }

    public class DtoToSeedProfile : Profile
    {
        public DtoToSeedProfile() => CreateMap<SeedDto, Data.Entities.Resource.Seed>().MaxDepth(3);
    }
}