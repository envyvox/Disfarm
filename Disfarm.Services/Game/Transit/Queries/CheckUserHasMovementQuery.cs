using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Transit.Queries
{
    public record CheckUserHasMovementQuery(long UserId) : IRequest<bool>;

    public class CheckUserHasMovementHandler : IRequestHandler<CheckUserHasMovementQuery, bool>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CheckUserHasMovementHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<bool> Handle(CheckUserHasMovementQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await db.UserMovements
                .AnyAsync(x => x.UserId == request.UserId);
        }
    }
}