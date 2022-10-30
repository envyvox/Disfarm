using System;
using AutoMapper;
using Disfarm.Data.Entities.User;

namespace Disfarm.Services.Game.Container.Models
{
	public record UserContainerDto(
		Guid Id,
		Data.Enums.Container Type,
		uint Amount,
		DateTimeOffset CreatedAt,
		DateTimeOffset UpdatedAt);

	public class UserContainerToDtoProfile : Profile
	{
		public UserContainerToDtoProfile() => CreateMap<UserContainer, UserContainerDto>();
	}

	public class DtoToUserContainerProfile : Profile
	{
		public DtoToUserContainerProfile() => CreateMap<UserContainerDto, UserContainer>();
	}
}