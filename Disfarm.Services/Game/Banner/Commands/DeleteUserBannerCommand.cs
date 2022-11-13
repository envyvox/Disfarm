using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Banner.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Banner.Commands
{
    public record DeleteUserBannerCommand(long UserId, Guid BannerId) : IRequest;

    public class DeleteUserBannerHandler : IRequestHandler<DeleteUserBannerCommand>
    {
        private readonly ILogger<DeleteUserBannerHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _scopeFactory;

        public DeleteUserBannerHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<DeleteUserBannerHandler> logger,
            IMediator mediator)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteUserBannerCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync(db.UserBanners, x =>
                x.UserId == request.UserId &&
                x.BannerId == request.BannerId);

            if (entity is null)
            {
                throw new Exception(
                    $"user {request.UserId} doesnt have banner {request.BannerId}");
            }

            await db.DeleteEntity(entity);

            _logger.LogInformation(
                "Deleted user banner entity {@Entity}",
                entity);

            if (entity.IsActive)
            {
                var banners = await _mediator.Send(new GetBannersQuery());
                var banner = banners.Single(x => x.Name == "Ночной город");

                await _mediator.Send(new ActivateUserBannerCommand(request.UserId, banner.Id));
            }

            return Unit.Value;
        }
    }
}