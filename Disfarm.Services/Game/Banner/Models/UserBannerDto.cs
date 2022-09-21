using System;
using AutoMapper;
using Disfarm.Data.Entities.User;

namespace Disfarm.Services.Game.Banner.Models
{
    public record UserBannerDto(
        Guid Id,
        BannerDto Banner,
        bool IsActive,
        DateTimeOffset Expiration,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);

    public class UserBannerToDtoProfile : Profile
    {
        public UserBannerToDtoProfile() => CreateMap<UserBanner, UserBannerDto>();
    }

    public class DtoToUserBannerProfile : Profile
    {
        public DtoToUserBannerProfile() => CreateMap<UserBannerDto, UserBanner>();
    }
}