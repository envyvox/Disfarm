using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.World.Queries
{
    public record GetWorldPropertyValueQuery(WorldProperty Type) : IRequest<uint>;

    public class GetWorldPropertyValueHandler : IRequestHandler<GetWorldPropertyValueQuery, uint>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public GetWorldPropertyValueHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<uint> Handle(GetWorldPropertyValueQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.WorldProperties
                .SingleOrDefaultAsync(x => x.Type == request.Type);

            if (entity is null)
            {
                throw new Exception(
                    $"world property {request.Type.ToString()} doesnt exist");
            }

            return entity.Value;
        }
    }
}