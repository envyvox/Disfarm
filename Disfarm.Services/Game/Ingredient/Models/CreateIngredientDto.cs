using Disfarm.Data.Enums;

namespace Disfarm.Services.Game.Ingredient.Models
{
    public record CreateIngredientDto(
        IngredientCategory Category,
        string Name,
        uint Amount);
}