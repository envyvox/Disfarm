using System;
using AutoMapper;
using Disfarm.Data.Entities.User;

namespace Disfarm.Services.Game.Cooldown.Models
{
    public record UserCooldownDto(
        long UserId,
        Data.Enums.Cooldown Type,
        DateTimeOffset Expiration);

    public class UserCooldownProfile : Profile
    {
        public UserCooldownProfile() => CreateMap<UserCooldown, UserCooldownDto>();
    }
}