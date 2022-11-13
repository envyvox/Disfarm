using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Effect.Commands
{
    public record RemoveEffectFromUsersCommand(Data.Enums.Effect Effect) : IRequest;

    public class RemoveEffectFromUsersHandler : IRequestHandler<RemoveEffectFromUsersCommand>
    {
        private readonly ILogger<RemoveEffectFromUsersHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public RemoveEffectFromUsersHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<RemoveEffectFromUsersHandler> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveEffectFromUsersCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await db.Database.ExecuteSqlInterpolatedAsync(@$"
                delete from user_effects
                where type = {request.Effect};");

            _logger.LogInformation(
                "Deleted user effect entities where effect {Effect}",
                request.Effect.ToString());

            return Unit.Value;
        }
    }
}