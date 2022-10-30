using System;
using AutoMapper;
using Disfarm.Data.Entities;
using Disfarm.Data.Enums;

namespace Disfarm.Services.Game.World.Models
{
	public record WorldStateDto(
		Guid Id,
		Season CurrentSeason,
		Weather WeatherToday,
		Weather WeatherTomorrow,
		DateTimeOffset CreatedAt,
		DateTimeOffset UpdatedAt);

	public class WorldStateToDtoProfile : Profile
	{
		public WorldStateToDtoProfile() => CreateMap<WorldState, WorldStateDto>();
	}

	public class DtoToWorldStateProfile : Profile
	{
		public DtoToWorldStateProfile() => CreateMap<WorldStateDto, WorldState>();
	}
}