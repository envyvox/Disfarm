using System;
using AutoMapper;
using Disfarm.Data.Entities.Resource.Ingredient;
using Disfarm.Data.Enums;

namespace Disfarm.Services.Game.Food.Models
{
    public record FoodIngredientDto(
        Guid Id,
        IngredientCategory Category,
        Guid IngredientId,
        uint Amount,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt,
        FoodDto Food);

    public class FoodIngredientToDtoProfile : Profile
    {
        public FoodIngredientToDtoProfile() => CreateMap<FoodIngredient, FoodIngredientDto>().MaxDepth(3);
    }

    public class DtoToFoodIngredientProfile : Profile
    {
        public DtoToFoodIngredientProfile() => CreateMap<FoodIngredientDto, FoodIngredient>().MaxDepth(3);
    }
}