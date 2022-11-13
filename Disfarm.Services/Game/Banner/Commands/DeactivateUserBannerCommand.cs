using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Banner.Commands
{
    public record DeactivateUserBannerCommand(long UserId, Guid BannerId) : IRequest;

    public class DeactivateUserBannerHandler : IRequestHandler<DeactivateUserBannerCommand>
    {
        private readonly ILogger<DeactivateUserBannerHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public DeactivateUserBannerHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<DeactivateUserBannerHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(DeactivateUserBannerCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserBanners
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.BannerId == request.BannerId);

            entity.IsActive = false;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await db.UpdateEntity(entity);

            _logger.LogInformation(
                "Deactivated user {UserId} banner {BannerId}",
                request.UserId, request.BannerId);

            return Unit.Value;
        }
    }
}