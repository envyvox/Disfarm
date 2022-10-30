using System;
using AutoMapper;
using Disfarm.Data.Entities.User;

namespace Disfarm.Services.Game.Title.Models
{
	public record UserTitleDto(
		Guid Id,
		Data.Enums.Title Type,
		DateTimeOffset CreatedAt);

	public class UserTitleToDtoProfile : Profile
	{
		public UserTitleToDtoProfile() => CreateMap<UserTitle, UserTitleDto>();
	}

	public class DtoToUserTitleProfile : Profile
	{
		public DtoToUserTitleProfile() => CreateMap<UserTitleDto, UserTitle>();
	}
}