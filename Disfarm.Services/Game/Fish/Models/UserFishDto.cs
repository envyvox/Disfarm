using System;
using AutoMapper;
using Disfarm.Data.Entities.User;

namespace Disfarm.Services.Game.Fish.Models
{
	public record UserFishDto(
		Guid Id,
		FishDto Fish,
		uint Amount,
		DateTimeOffset CreatedAt,
		DateTimeOffset UpdatedAt);

	public class UserFishToDtoProfile : Profile
	{
		public UserFishToDtoProfile() => CreateMap<UserFish, UserFishDto>();
	}

	public class DtoToUserFishProfile : Profile
	{
		public DtoToUserFishProfile() => CreateMap<UserFishDto, UserFish>();
	}
}