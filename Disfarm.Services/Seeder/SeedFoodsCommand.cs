using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Disfarm.Services.Game.Food.Commands;
using Disfarm.Services.Game.Ingredient.Models;
using MediatR;

namespace Disfarm.Services.Seeder
{
    public record SeedFoodsCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeedFoodsHandler : IRequestHandler<SeedFoodsCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;

        public SeedFoodsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeedFoodsCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new CreateFoodCommand[]
            {
                new("Tortilla", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Corn", 1)
                }),
                new("Bread", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Product, "WheatFlour", 1)
                }),
                new("FriedEgg", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Product, "Egg", 1)
                }),
                new("BeanHotpot", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "GreenBean", 2)
                }),
                new("Spaghetti", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Product, "WheatFlour", 1)
                }),
                new("VegetableMedley", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Tomato", 1),
                    new(IngredientCategory.Crop, "Beet", 1)
                }),
                new("EggplantParmesan", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Eggplant", 1),
                    new(IngredientCategory.Crop, "Tomato", 1)
                }),
                new("Pancakes", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Product, "Egg", 1),
                    new(IngredientCategory.Product, "WheatFlour", 1)
                }),
                new("Bruschetta", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Tomato", 1),
                    new(IngredientCategory.Product, "Oil", 1),
                    new(IngredientCategory.Food, "Bread", 1)
                }),
                new("Hashbrowns", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Potato", 1),
                    new(IngredientCategory.Product, "Oil", 1)
                }),
                new("RedPlate", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "RedCabbage", 1),
                    new(IngredientCategory.Crop, "Radish", 1)
                }),
                new("Omelet", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Product, "Egg", 1),
                    new(IngredientCategory.Product, "Milk", 1),
                    new(IngredientCategory.Crop, "Eggplant", 1)
                }),
                new("PepperPoppers", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "HotPepper", 1),
                    new(IngredientCategory.Product, "Cheese", 1)
                }),
                new("GarlicOil", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Garlic", 1),
                    new(IngredientCategory.Product, "Oil", 1)
                }),
                new("RadishSalad", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Radish", 1),
                    new(IngredientCategory.Product, "Oil", 1),
                    new(IngredientCategory.Product, "Vinegar", 1)
                }),
                new("FarmBreakfast", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Parsnip", 1),
                    new(IngredientCategory.Food, "Omelet", 1)
                }),
                new("SuperMeal", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "BokChoy", 1),
                    new(IngredientCategory.Crop, "Cranberry", 1),
                    new(IngredientCategory.Crop, "Artichoke", 1)
                }),
                new("Pizza", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Tomato", 1),
                    new(IngredientCategory.Product, "WheatFlour", 1),
                    new(IngredientCategory.Product, "Cheese", 1)
                }),
                new("ArtichokeDip", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Artichoke", 1),
                    new(IngredientCategory.Product, "Milk", 1)
                }),
                new("ParsnipSoup", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Parsnip", 1),
                    new(IngredientCategory.Product, "Milk", 1),
                    new(IngredientCategory.Product, "Vinegar", 1)
                }),
                new("CranberrySauce", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Cranberry", 1),
                    new(IngredientCategory.Product, "Sugar", 1)
                }),
                new("CheeseCauliflower", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Cauliflower", 1),
                    new(IngredientCategory.Product, "Cheese", 1)
                }),
                new("Coleslaw", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "RedCabbage", 1),
                    new(IngredientCategory.Product, "Vinegar", 1),
                    new(IngredientCategory.Product, "Mayonnaise", 1)
                }),
                new("Cookie", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Product, "Egg", 1),
                    new(IngredientCategory.Product, "WheatFlour", 1),
                    new(IngredientCategory.Product, "Sugar", 1)
                }),
                new("ChocolateCake", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Product, "Egg", 1),
                    new(IngredientCategory.Product, "WheatFlour", 1),
                    new(IngredientCategory.Product, "Sugar", 1)
                }),
                new("IceCream", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Product, "Milk", 1),
                    new(IngredientCategory.Product, "Sugar", 1)
                }),
                new("GlazedYams", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Yam", 1),
                    new(IngredientCategory.Product, "Sugar", 1)
                }),
                new("BlueberryTart", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Blueberry", 1),
                    new(IngredientCategory.Product, "Egg", 1),
                    new(IngredientCategory.Product, "WheatFlour", 1),
                    new(IngredientCategory.Product, "Sugar", 1)
                }),
                new("PumpkinSoup", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Pumpkin", 1),
                    new(IngredientCategory.Product, "Milk", 1)
                }),
                new("AutumnTale", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Yam", 1),
                    new(IngredientCategory.Crop, "Pumpkin", 1)
                }),
                new("CompleteBreakfast", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Product, "Milk", 1),
                    new(IngredientCategory.Food, "FriedEgg", 1),
                    new(IngredientCategory.Food, "Hashbrowns", 1),
                    new(IngredientCategory.Food, "Pancakes", 1)
                }),
                new("RhubarbPie", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Rhubarb", 1),
                    new(IngredientCategory.Product, "WheatFlour", 1),
                    new(IngredientCategory.Product, "Sugar", 1)
                }),
                new("PinkCake", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Melon", 1),
                    new(IngredientCategory.Product, "Egg", 1),
                    new(IngredientCategory.Product, "WheatFlour", 1),
                    new(IngredientCategory.Product, "Sugar", 1)
                }),
                new("PumpkinPie", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Pumpkin", 1),
                    new(IngredientCategory.Product, "Milk", 1),
                    new(IngredientCategory.Product, "WheatFlour", 1),
                    new(IngredientCategory.Product, "Sugar", 1)
                }),
                new("Onigiri", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Rice", 1)
                }),
                new("CurryRice", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Rice", 1),
                    new(IngredientCategory.Crop, "HotPepper", 1),
                    new(IngredientCategory.Crop, "Tomato", 1)
                }),
                new("Oyakodon", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Food, "Omelet", 1),
                    new(IngredientCategory.Crop, "Rice", 1)
                }),
                new("ParsnipChips", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Parsnip", 1),
                    new(IngredientCategory.Product, "Oil", 1)
                }),
                new("GarlicRice", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Rice", 1),
                    new(IngredientCategory.Crop, "Garlic", 1)
                }),
                new("HerbsEggs", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Food, "FriedEgg", 1),
                    new(IngredientCategory.Crop, "GreenBean", 1),
                    new(IngredientCategory.Crop, "Cauliflower", 1)
                }),
                new("SpringSalad", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Parsnip", 1),
                    new(IngredientCategory.Crop, "Rhubarb", 1),
                    new(IngredientCategory.Crop, "GreenBean", 1)
                }),
                new("CreamSoup", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Garlic", 1),
                    new(IngredientCategory.Crop, "Cauliflower", 1),
                    new(IngredientCategory.Product, "Oil", 1),
                    new(IngredientCategory.Food, "Bread", 1)
                }),
                new("BakedGarlic", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Garlic", 1),
                    new(IngredientCategory.Product, "Oil", 1),
                    new(IngredientCategory.Food, "Bread", 1)
                }),
                new("RiceBalls", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Rice", 1),
                    new(IngredientCategory.Product, "Cheese", 1),
                    new(IngredientCategory.Crop, "Potato", 1)
                }),
                new("CreamyPuree", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Potato", 1),
                    new(IngredientCategory.Product, "Milk", 1),
                    new(IngredientCategory.Crop, "Parsnip", 1),
                    new(IngredientCategory.Product, "Oil", 1)
                }),
                new("MochiCakes", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Product, "Vinegar", 1),
                    new(IngredientCategory.Product, "Sugar", 1),
                    new(IngredientCategory.Product, "WheatFlour", 1),
                    new(IngredientCategory.Crop, "Strawberry", 1)
                }),
                new("HokkaidoBuns", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Strawberry", 1),
                    new(IngredientCategory.Product, "WheatFlour", 1),
                    new(IngredientCategory.Product, "Milk", 1),
                    new(IngredientCategory.Crop, "Rhubarb", 1)
                }),
                new("FruitIce", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Product, "Sugar", 1),
                    new(IngredientCategory.Crop, "Strawberry", 1)
                }),
                new("ChakinSibori", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "GreenBean", 1),
                    new(IngredientCategory.Product, "Sugar", 1),
                    new(IngredientCategory.Product, "Egg", 1)
                }),
                new("GlazedParsnip", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Parsnip", 1),
                    new(IngredientCategory.Product, "Sugar", 1),
                    new(IngredientCategory.Product, "Vinegar", 1)
                }),
                new("StrawberryCake", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Product, "WheatFlour", 1),
                    new(IngredientCategory.Crop, "Strawberry", 1),
                    new(IngredientCategory.Product, "Sugar", 1)
                }),
                new("StrawberryRhubarbJam", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Strawberry", 1),
                    new(IngredientCategory.Product, "Sugar", 1),
                    new(IngredientCategory.Crop, "Rhubarb", 1)
                }),
                new("Tofu", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "GreenBean", 1),
                    new(IngredientCategory.Product, "Cheese", 1)
                }),
                new("GlazedMelon", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Melon", 1),
                    new(IngredientCategory.Product, "Sugar", 1)
                }),
                new("MelonBlueberrySorbet", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Melon", 1),
                    new(IngredientCategory.Product, "Sugar", 1),
                    new(IngredientCategory.Crop, "Blueberry", 1)
                }),
                new("PickledMelon", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Melon", 1),
                    new(IngredientCategory.Crop, "HotPepper", 1),
                    new(IngredientCategory.Product, "Oil", 1)
                }),
                new("PumpkinMelonCake", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Melon", 1),
                    new(IngredientCategory.Crop, "Pumpkin", 1),
                    new(IngredientCategory.Product, "Egg", 1),
                    new(IngredientCategory.Product, "WheatFlour", 1)
                }),
                new("SpicyTofu", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Food, "Tofu", 1),
                    new(IngredientCategory.Crop, "HotPepper", 1),
                    new(IngredientCategory.Crop, "Corn", 1),
                    new(IngredientCategory.Product, "Oil", 1)
                }),
                new("RedSalad", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "RedCabbage", 1),
                    new(IngredientCategory.Crop, "Corn", 1),
                    new(IngredientCategory.Crop, "Tomato", 1),
                    new(IngredientCategory.Product, "Oil", 1)
                }),
                new("RedSaladOmelet", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Food, "RedSalad", 1),
                    new(IngredientCategory.Food, "Omelet", 1)
                }),
                new("CornSoup", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Corn", 1),
                    new(IngredientCategory.Product, "Milk", 1),
                    new(IngredientCategory.Product, "WheatFlour", 1),
                    new(IngredientCategory.Product, "Oil", 1)
                }),
                new("Burrito", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Food, "Tortilla", 1),
                    new(IngredientCategory.Crop, "HotPepper", 1),
                    new(IngredientCategory.Crop, "Corn", 1),
                    new(IngredientCategory.Crop, "Tomato", 1)
                }),
                new("CornSalad", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Corn", 1),
                    new(IngredientCategory.Product, "Mayonnaise", 1),
                    new(IngredientCategory.Product, "Egg", 1)
                }),
                new("Harunosarada", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Corn", 1),
                    new(IngredientCategory.Crop, "RedCabbage", 1),
                    new(IngredientCategory.Crop, "Radish", 1),
                    new(IngredientCategory.Product, "Vinegar", 1)
                }),
                new("SpaghettiSalad", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Tomato", 1),
                    new(IngredientCategory.Food, "Spaghetti", 1),
                    new(IngredientCategory.Product, "Oil", 1)
                }),
                new("AgeTofu", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Tomato", 1),
                    new(IngredientCategory.Food, "Tofu", 1),
                    new(IngredientCategory.Product, "Oil", 1),
                    new(IngredientCategory.Product, "Sugar", 1)
                }),
                new("SpicySpaghetti", false, new List<CreateIngredientDto>
                {
                    new(IngredientCategory.Crop, "Tomato", 1),
                    new(IngredientCategory.Food, "Tofu", 1),
                    new(IngredientCategory.Food, "Spaghetti", 1),
                    new(IngredientCategory.Crop, "HotPepper", 1)
                }),
            };

            foreach (var createFoodCommand in commands)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(createFoodCommand);

                    result.Affected++;
                }
                catch
                {
                    // ignored
                }
            }

            return result;
        }
    }
}