using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.Ingredient.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Ingredient
{
    public record GetIngredientByNameQuery(IngredientCategory Category, string Name) : IRequest<IngredientDto>;

    public class GetIngredientByNameHandler : IRequestHandler<GetIngredientByNameQuery, IngredientDto>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public GetIngredientByNameHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<IngredientDto> Handle(GetIngredientByNameQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            switch (request.Category)
            {
                case IngredientCategory.Undefined:
                {
                    throw new ArgumentException("undefined ingredient category");
                }
                case IngredientCategory.Product:
                {
                    var entity = await db.Products.FirstOrDefaultAsync(x => x.Name == request.Name);

                    if (entity is null)
                    {
                        throw new Exception($"product with name {request.Name} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                case IngredientCategory.Crop:
                {
                    var entity = await db.Crops.FirstOrDefaultAsync(x => x.Name == request.Name);

                    if (entity is null)
                    {
                        throw new Exception($"crop with name {request.Name} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                case IngredientCategory.Food:
                {
                    var entity = await db.Foods.FirstOrDefaultAsync(x => x.Name == request.Name);

                    if (entity is null)
                    {
                        throw new Exception($"food with name {request.Name} not found");
                    }

                    return new IngredientDto(request.Category, entity.Id, entity.Name);
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}