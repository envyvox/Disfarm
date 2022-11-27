using AutoMapper;
using Disfarm.Data.Entities.User;

namespace Disfarm.Services.Game.Product.Models
{
	public record UserProductDto(
		ProductDto Product,
		uint Amount);

	public class UserProductToDtoProfile : Profile
	{
		public UserProductToDtoProfile() => CreateMap<UserProduct, UserProductDto>();
	}

	public class DtoToUserProductProfile : Profile
	{
		public DtoToUserProductProfile() => CreateMap<UserProductDto, UserProduct>();
	}
}