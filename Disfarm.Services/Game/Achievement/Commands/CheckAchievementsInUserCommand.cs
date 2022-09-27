using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Disfarm.Services.Game.Achievement.Commands
{
    public record CheckAchievementsInUserCommand(
            ulong GuildId,
            long UserId,
            IEnumerable<Data.Enums.Achievement> Achievements)
        : IRequest;

    public class CheckAchievementsInUserHandler : IRequestHandler<CheckAchievementsInUserCommand>
    {
        private readonly IMediator _mediator;

        public CheckAchievementsInUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckAchievementsInUserCommand request, CancellationToken ct)
        {
            foreach (var achievement in request.Achievements)
            {
                await _mediator.Send(new CheckAchievementInUserCommand(request.GuildId, request.UserId, achievement));
            }

            return Unit.Value;
        }
    }
}