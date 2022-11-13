using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Crop.Commands
{
    public record RemoveCropFromUserCommand(long UserId, Guid CropId, uint Amount) : IRequest;

    public class RemoveCropFromUserHandler : IRequestHandler<RemoveCropFromUserCommand>
    {
        private readonly ILogger<RemoveCropFromUserHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public RemoveCropFromUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<RemoveCropFromUserHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(RemoveCropFromUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserCrops
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.CropId == request.CropId);

            if (entity is null)
            {
                throw new Exception(
                    $"user {request.UserId} doesnt have entity with crop {request.CropId}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await db.UpdateEntity(entity);

            _logger.LogInformation(
                "Removed from user {UserId} crop {CropId} amount {Amount}",
                request.UserId, request.CropId, request.Amount);

            return Unit.Value;
        }
    }
}