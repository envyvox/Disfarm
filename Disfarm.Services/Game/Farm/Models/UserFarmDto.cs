using System;
using AutoMapper;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.Seed.Models;

namespace Disfarm.Services.Game.Farm.Models
{
    public record UserFarmDto(
        Guid Id,
        uint Number,
        FieldState State,
        SeedDto Seed,
        uint Progress,
        bool InReGrowth,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);

    public class UserFarmToDtoProfile : Profile
    {
        public UserFarmToDtoProfile() => CreateMap<UserFarm, UserFarmDto>();
    }

    public class DtoToUserFarmProfile : Profile
    {
        public DtoToUserFarmProfile() => CreateMap<UserFarmDto, UserFarm>();
    }
}