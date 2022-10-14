using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Effect.Queries
{
    public record CountUsersWithEffectQuery(Data.Enums.Effect Effect) : IRequest<uint>;
    
    public class CountUsersWithEffectHandler : IRequestHandler<CountUsersWithEffectQuery, uint>
    {
        private readonly AppDbContext _db;

        public CountUsersWithEffectHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<uint> Handle(CountUsersWithEffectQuery request, CancellationToken ct)
        {
            return (uint) await _db.UserEffects.CountAsync(x => x.Type == request.Effect);
        }
    }
}