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

namespace Disfarm.Services.Game.Achievement.Commands
{
    public record CreateAchievementCommand(
            Data.Enums.Achievement Type,
            AchievementRewardType RewardType,
            uint RewardNumber,
            uint Points)
        : IRequest;

    public class CreateAchievementHandler : IRequestHandler<CreateAchievementCommand>
    {
        private readonly ILogger<CreateAchievementHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public CreateAchievementHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<CreateAchievementHandler> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateAchievementCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var exist = await db.Achievements.AnyAsync(x => x.Type == request.Type);

            if (exist)
            {
                throw new Exception(
                    $"Achievement {request.Type.ToString()} already exist");
            }

            var created = await db.CreateEntity(new Data.Entities.Achievement
            {
                Type = request.Type,
                RewardType = request.RewardType,
                RewardNumber = request.RewardNumber,
                Points = request.Points,
                CreatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created achievement entity {@Entity}",
                created);

            return Unit.Value;
        }
    }
}