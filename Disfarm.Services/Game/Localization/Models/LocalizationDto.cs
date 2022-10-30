using System;
using AutoMapper;
using Disfarm.Data.Enums;

namespace Disfarm.Services.Game.Localization.Models
{
	public record LocalizationDto(
		Guid Id,
		LocalizationCategory Category,
		string Name,
		Language Language,
		string Single,
		string Double,
		string Multiply);

	public class LocalizationProfile : Profile
	{
		public LocalizationProfile() => CreateMap<Data.Entities.Localization, LocalizationDto>();
	}
}