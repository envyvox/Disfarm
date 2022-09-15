using System;
using AutoMapper;
using Disfarm.Data.Enums;

namespace Disfarm.Services.Game.Banner.Models
{
    public record BannerDto(
        Guid Id,
        string Name,
        BannerRarity Rarity,
        uint Price,
        string Url,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);

    public class BannerToDtoProfile : Profile
    {
        public BannerToDtoProfile() => CreateMap<Data.Entities.Banner, BannerDto>();
    }

    public class DtoToBannerProfile : Profile
    {
        public DtoToBannerProfile() => CreateMap<BannerDto, Data.Entities.Banner>();
    }
}