using System;
using System.Collections.Generic;
using AutoMapper;
using Disfarm.Data.Enums;

namespace Disfarm.Services.Game.Fish.Models
{
    public record FishDto(
        Guid Id,
        string Name,
        FishRarity Rarity,
        Weather CatchWeather,
        TimesDayType CatchTimesDay,
        List<Season> CatchSeasons,
        uint Price,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);

    public class FishToDtoProfile : Profile
    {
        public FishToDtoProfile() => CreateMap<Data.Entities.Fish, FishDto>();
    }

    public class DtoToFishProfile : Profile
    {
        public DtoToFishProfile() => CreateMap<FishDto, Data.Entities.Fish>();
    }
}