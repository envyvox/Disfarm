using System;
using AutoMapper;
using Disfarm.Data.Entities.User;

namespace Disfarm.Services.Game.Crop.Models
{
	public record UserCropDto(
		Guid Id,
		CropDto Crop,
		uint Amount,
		DateTimeOffset CreatedAt,
		DateTimeOffset UpdatedAt);

	public class UserCropToDtoProfile : Profile
	{
		public UserCropToDtoProfile() => CreateMap<UserCrop, UserCropDto>();
	}

	public class DtoToUserCropProfile : Profile
	{
		public DtoToUserCropProfile() => CreateMap<UserCropDto, UserCrop>();
	}
}