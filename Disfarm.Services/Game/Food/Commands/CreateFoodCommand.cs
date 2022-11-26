using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Entities.Resource.Ingredient;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Ingredient;
using Disfarm.Services.Game.Ingredient.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Food.Commands
{
    public record CreateFoodCommand(
            string Name,
            bool RecipeSellable,
            List<CreateIngredientDto> Ingredients)
        : IRequest;

    public class CreateFoodHandler : IRequestHandler<CreateFoodCommand>
    {
        private readonly ILogger<CreateFoodHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMediator _mediator;

        public CreateFoodHandler(
            ILogger<CreateFoodHandler> logger,
            IServiceScopeFactory scopeFactory,
            IMediator mediator)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateFoodCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var exist = await db.Foods.AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception(
                    $"food with name {request.Name} already exist");
            }

            var createdFood = await db.CreateEntity(new Data.Entities.Resource.Food
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                RecipeSellable = request.RecipeSellable,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created food entity {@Entity}",
                createdFood);

            foreach (var createIngredient in request.Ingredients)
            {
                var ingredient = await _mediator.Send(new GetIngredientByNameQuery(
                    createIngredient.Category, createIngredient.Name));

                var createdIngredient = await db.CreateEntity(new FoodIngredient
                {
                    Id = Guid.NewGuid(),
                    Category = ingredient.Category,
                    IngredientId = ingredient.Id,
                    Amount = createIngredient.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    FoodId = createdFood.Id
                });

                _logger.LogInformation(
                    "Created food ingredient entity {@Entity}",
                    createdIngredient);
            }

            return Unit.Value;
        }
    }
}