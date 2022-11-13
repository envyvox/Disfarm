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
    public record ActivateUserBannerCommand(long UserId, Guid BannerId) : IRequest;

    public class ActivateUserBannerHandler : IRequestHandler<ActivateUserBannerCommand>
    {
        private readonly ILogger<ActivateUserBannerHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public ActivateUserBannerHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<ActivateUserBannerHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(ActivateUserBannerCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserBanners
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.BannerId == request.BannerId);

            entity.IsActive = true;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await db.UpdateEntity(entity);

            _logger.LogInformation(
                "Activated user {UserId} banner {BannerId}",
                request.UserId, request.BannerId);

            return Unit.Value;
        }
    }
}