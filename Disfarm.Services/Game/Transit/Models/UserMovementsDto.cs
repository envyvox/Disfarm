using System;
using AutoMapper;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Enums;

namespace Disfarm.Services.Game.Transit.Models
{
	public record UserMovementDto(
		Guid Id,
		Location Departure,
		Location Destination,
		DateTimeOffset Arrival,
		DateTimeOffset CreatedAt);

	public class UserMovementToDtoProfile : Profile
	{
		public UserMovementToDtoProfile() => CreateMap<UserMovement, UserMovementDto>();
	}

	public class DtoToUserMovementProfile : Profile
	{
		public DtoToUserMovementProfile() => CreateMap<UserMovementDto, UserMovement>();
	}
}