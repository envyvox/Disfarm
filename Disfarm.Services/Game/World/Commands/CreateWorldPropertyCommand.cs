using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.World.Commands
{
    public record CreateWorldPropertyCommand(WorldProperty Type, uint Value) : IRequest;

    public class CreateWorldPropertyHandler : IRequestHandler<CreateWorldPropertyCommand>
    {
        private readonly ILogger<CreateWorldPropertyHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public CreateWorldPropertyHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<CreateWorldPropertyHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(CreateWorldPropertyCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var exist = await db.WorldProperties
                .AnyAsync(x => x.Type == request.Type);

            if (exist)
            {
                throw new Exception(
                    $"world property {request.Type.ToString()} already exist");
            }

            var created = await db.CreateEntity(new Data.Entities.WorldProperty
            {
                Type = request.Type,
                Value = request.Value,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created world property entity {@Entity}",
                created);

            return Unit.Value;
        }
    }
}