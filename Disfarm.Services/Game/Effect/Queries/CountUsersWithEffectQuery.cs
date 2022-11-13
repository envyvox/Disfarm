using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Effect.Queries
{
    public record CountUsersWithEffectQuery(Data.Enums.Effect Effect) : IRequest<uint>;

    public class CountUsersWithEffectHandler : IRequestHandler<CountUsersWithEffectQuery, uint>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CountUsersWithEffectHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<uint> Handle(CountUsersWithEffectQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return (uint)await db.UserEffects.CountAsync(x => x.Type == request.Effect);
        }
    }
}