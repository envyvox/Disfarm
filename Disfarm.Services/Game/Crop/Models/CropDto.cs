using System;
using AutoMapper;
using Disfarm.Services.Game.Seed.Models;

namespace Disfarm.Services.Game.Crop.Models
{
	public record CropDto(
		Guid Id,
		string Name,
		uint Price,
		SeedDto Seed,
		DateTimeOffset CreatedAt,
		DateTimeOffset UpdatedAt);

	public class CropToDtoProfile : Profile
	{
		public CropToDtoProfile() => CreateMap<Data.Entities.Resource.Crop, CropDto>().MaxDepth(3);
	}

	public class DtoToCropProfile : Profile
	{
		public DtoToCropProfile() => CreateMap<CropDto, Data.Entities.Resource.Crop>().MaxDepth(3);
	}
}