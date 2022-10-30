using System;
using AutoMapper;
using Disfarm.Data.Entities.User;

namespace Disfarm.Services.Game.Achievement.Models
{
	public record UserAchievementDto(
		Guid Id,
		AchievementDto Achievement,
		DateTimeOffset CreatedAt);

	public class UserAchievementToDtoProfile : Profile
	{
		public UserAchievementToDtoProfile() => CreateMap<UserAchievement, UserAchievementDto>();
	}

	public class DtoToUserAchievementProfile : Profile
	{
		public DtoToUserAchievementProfile() => CreateMap<UserAchievementDto, UserAchievement>();
	}
}