using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Effect.Queries
{
    public record CheckUserHasEffectQuery(long UserId, Data.Enums.Effect Effect) : IRequest<bool>;

    public class CheckUserHasEffectHandler : IRequestHandler<CheckUserHasEffectQuery, bool>
    {
        private readonly AppDbContext _db;

        public CheckUserHasEffectHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<bool> Handle(CheckUserHasEffectQuery request, CancellationToken ct)
        {
            return await _db.UserEffects.AnyAsync(x =>
                x.UserId == request.UserId &&
                x.Type == request.Effect);
        }
    }
}