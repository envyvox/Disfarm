using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Achievement.Commands
{
    public record CreateUserAchievementCommand(
            long UserId,
            Data.Enums.Achievement.Achievement Type)
        : IRequest;

    public class CreateUserAchievementHandler : IRequestHandler<CreateUserAchievementCommand>
    {
        private readonly ILogger<CreateUserAchievementHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;

        public CreateUserAchievementHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<CreateUserAchievementHandler> logger,
            IMediator mediator,
            IMemoryCache cache)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _mediator = mediator;
            _cache = cache;
        }

        public async Task<Unit> Handle(CreateUserAchievementCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var exist = await db.UserAchievements
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (exist)
            {
                throw new Exception(
                    $"user {request.UserId} already have achievement {request.Type.ToString()}");
            }

            var created = await db.CreateEntity(new UserAchievement
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Type = request.Type,
                CreatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created user achievement entity {@Entity}",
                created);

            _cache.Set(CacheExtensions.GetUserHasAchievementKey(request.UserId, request.Type), true,
                CacheExtensions.DefaultCacheOptions);

            return await _mediator.Send(new AddAchievementRewardToUserCommand(request.UserId, request.Type));
        }
    }
}