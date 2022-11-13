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

namespace Disfarm.Services.Game.Banner.Commands
{
    public record AddBannerToUserCommand(
            long UserId,
            Guid BannerId,
            TimeSpan? Duration,
            bool IsActive = false)
        : IRequest;

    public class AddBannerToUserHandler : IRequestHandler<AddBannerToUserCommand>
    {
        private readonly ILogger<AddBannerToUserHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public AddBannerToUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<AddBannerToUserHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(AddBannerToUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserBanners
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.BannerId == request.BannerId);

            if (entity is null)
            {
                var created = await db.CreateEntity(new UserBanner
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    BannerId = request.BannerId,
                    IsActive = request.IsActive,
                    Expiration = DateTimeOffset.UtcNow.Add(request.Duration ?? TimeSpan.Zero),
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user banner entity {@Entity}",
                    created);
            }
            else
            {
                entity.Expiration = entity.Expiration?.Add(request.Duration ?? TimeSpan.Zero);
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} duration {Duration} for banner {BannerId}",
                    request.UserId, request.Duration, request.BannerId);
            }

            return Unit.Value;
        }
    }
}