using System;
using AutoMapper;
using Disfarm.Data.Entities.User;

namespace Disfarm.Services.Game.Seed.Models
{
    public record UserSeedDto(
        Guid Id,
        SeedDto Seed,
        uint Amount,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);

    public class UserSeedToDtoProfile : Profile
    {
        public UserSeedToDtoProfile() => CreateMap<UserSeed, UserSeedDto>();
    }

    public class DtoToUserSeedProfile : Profile
    {
        public DtoToUserSeedProfile() => CreateMap<UserSeedDto, UserSeed>();
    }
}