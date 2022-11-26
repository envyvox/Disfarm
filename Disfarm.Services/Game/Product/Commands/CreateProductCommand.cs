using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Product.Commands
{
    public record CreateProductCommand(string Name, uint Price) : IRequest;

    public class CreateProductHandler : IRequestHandler<CreateProductCommand>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CreateProductHandler> _logger;

        public CreateProductHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<CreateProductHandler> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var exist = await db.Products.AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception($"product with name {request.Name} already exist");
            }

            var created = await db.CreateEntity(new Data.Entities.Resource.Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created product entity {@Entity}",
                created);

            return Unit.Value;
        }
    }
}