using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Transit.Queries
{
    public record CheckUserHasMovementQuery(long UserId) : IRequest<bool>;

    public class CheckUserHasMovementHandler : IRequestHandler<CheckUserHasMovementQuery, bool>
    {
        private readonly AppDbContext _db;

        public CheckUserHasMovementHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<bool> Handle(CheckUserHasMovementQuery request, CancellationToken ct)
        {
            return await _db.UserMovements
                .AnyAsync(x => x.UserId == request.UserId);
        }
    }
}