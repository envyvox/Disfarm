using System;
using AutoMapper;
using Disfarm.Data.Entities.User;

namespace Disfarm.Services.Game.Currency.Models
{
	public record UserCurrencyDto(
		Guid Id,
		Data.Enums.Currency Type,
		uint Amount,
		DateTimeOffset CreatedAt,
		DateTimeOffset UpdatedAt);

	public class UserCurrencyToDtoProfile : Profile
	{
		public UserCurrencyToDtoProfile() => CreateMap<UserCurrency, UserCurrencyDto>();
	}

	public class DtoToUserCurrencyProfile : Profile
	{
		public DtoToUserCurrencyProfile() => CreateMap<UserCurrencyDto, UserCurrency>();
	}
}