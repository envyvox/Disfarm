using System;
using AutoMapper;
using Disfarm.Data.Entities.User;
using Disfarm.Services.Game.User.Models;

namespace Disfarm.Services.Game.Effect.Models
{
	public record UserEffectDto(
		UserDto User,
		Data.Enums.Effect Type,
		DateTimeOffset? Expiration);

	public class UserEffectProfile : Profile
	{
		public UserEffectProfile() => CreateMap<UserEffect, UserEffectDto>();
	}
}