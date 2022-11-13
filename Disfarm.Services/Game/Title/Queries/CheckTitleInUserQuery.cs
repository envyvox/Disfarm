using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Title.Queries
{
    public record CheckTitleInUserQuery(long UserId, Data.Enums.Title Type) : IRequest<bool>;

    public class CheckTitleInUserHandler : IRequestHandler<CheckTitleInUserQuery, bool>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CheckTitleInUserHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<bool> Handle(CheckTitleInUserQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            return await db.UserTitles
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);
        }
    }
}