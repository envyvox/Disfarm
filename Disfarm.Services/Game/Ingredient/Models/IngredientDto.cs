using System;
using Disfarm.Data.Enums;

namespace Disfarm.Services.Game.Ingredient.Models
{
    public record IngredientDto(
        IngredientCategory Category,
        Guid Id,
        string Name);
}