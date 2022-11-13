using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Effect.Queries
{
    public record CheckUserHasEffectQuery(long UserId, Data.Enums.Effect Effect) : IRequest<bool>;

    public class CheckUserHasEffectHandler : IRequestHandler<CheckUserHasEffectQuery, bool>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CheckUserHasEffectHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<bool> Handle(CheckUserHasEffectQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await db.UserEffects.AnyAsync(x =>
                x.UserId == request.UserId &&
                x.Type == request.Effect);
        }
    }
}