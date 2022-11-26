using System;
using System.Collections.Generic;
using AutoMapper;

namespace Disfarm.Services.Game.Food.Models
{
    public record FoodDto(
        Guid Id,
        string Name,
        bool RecipeSellable,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt,
        List<FoodIngredientDto> Ingredients);

    public class FoodToDtoProfile : Profile
    {
        public FoodToDtoProfile() => CreateMap<Data.Entities.Resource.Food, FoodDto>().MaxDepth(3);
    }

    public class DtoToFoodProfile : Profile
    {
        public DtoToFoodProfile() => CreateMap<FoodDto, Data.Entities.Resource.Food>().MaxDepth(3);
    }
}