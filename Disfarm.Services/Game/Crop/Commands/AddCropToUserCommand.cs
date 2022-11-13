using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Crop.Commands
{
    public record AddCropToUserCommand(long UserId, Guid CropId, uint Amount) : IRequest;

    public class AddCropToUserHandler : IRequestHandler<AddCropToUserCommand>
    {
        private readonly ILogger<AddCropToUserHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public AddCropToUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<AddCropToUserHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(AddCropToUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserCrops
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.CropId == request.CropId);

            if (entity is null)
            {
                var created = await db.CreateEntity(new UserCrop
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    CropId = request.CropId,
                    Amount = request.Amount,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user crop entity {@Entity}",
                    created);
            }
            else
            {
                entity.Amount += request.Amount;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} crop {CropId} amount {Amount}",
                    request.UserId, request.CropId, request.Amount);
            }

            return Unit.Value;
        }
    }
}