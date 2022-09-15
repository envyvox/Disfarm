using System;
using AutoMapper;
using Disfarm.Data.Enums;

namespace Disfarm.Services.Game.User.Models
{
    public record UserDto(
        long Id,
        string About,
        uint Level,
        uint Xp,
        Data.Enums.Title Title,
        Gender Gender,
        Fraction Fraction,
        Location Location,
        string CommandColor,
        bool IsPremium,
        Language Language,
        CubeType CubeType,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);

    public class UserToDtoProfile : Profile
    {
        public UserToDtoProfile() => CreateMap<Data.Entities.User.User, UserDto>();
    }

    public class DtoToUserProfile : Profile
    {
        public DtoToUserProfile() => CreateMap<UserDto, Data.Entities.User.User>();
    }
}