using System;
using AutoMapper;
using Disfarm.Data.Enums;

namespace Disfarm.Services.Game.World.Models
{
    public record WorldPropertyDto(
        WorldProperty Type,
        uint Value,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);

    public class WorldPropertyToDtoProfile : Profile
    {
        public WorldPropertyToDtoProfile() => CreateMap<Data.Entities.WorldProperty, WorldPropertyDto>();
    }

    public class DtoToWorldPropertyProfile : Profile
    {
        public DtoToWorldPropertyProfile() => CreateMap<WorldPropertyDto, Data.Entities.WorldProperty>();
    }
}