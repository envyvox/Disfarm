using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Achievement.Commands
{
    public record CreateUserAchievementCommand(
            ulong GuildId,
            ulong ChannelId,
            long UserId,
            Data.Enums.Achievement Type)
        : IRequest;

    public class CreateUserAchievementHandler : IRequestHandler<CreateUserAchievementCommand>
    {
        private readonly ILogger<CreateUserAchievementHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _db;

        public CreateUserAchievementHandler(
            DbContextOptions options,
            ILogger<CreateUserAchievementHandler> logger,
            IMediator mediator,
            IMemoryCache cache)
        {
            _db = new AppDbContext(options);
            _logger = logger;
            _mediator = mediator;
            _cache = cache;
        }

        public async Task<Unit> Handle(CreateUserAchievementCommand request, CancellationToken ct)
        {
            var exist = await _db.UserAchievements
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (exist)
            {
                throw new Exception(
                    $"user {request.UserId} already have achievement {request.Type.ToString()}");
            }

            var created = await _db.CreateEntity(new UserAchievement
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

            return await _mediator.Send(new AddAchievementRewardToUserCommand(
                request.GuildId, request.ChannelId, request.UserId, request.Type));
        }
    }
}