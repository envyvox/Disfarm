using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.Collection.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Collection.Queries
{
    public record GetUserCollectionsQuery(long UserId, CollectionCategory Category) : IRequest<List<UserCollectionDto>>;

    public class GetUserCollectionsHandler : IRequestHandler<GetUserCollectionsQuery, List<UserCollectionDto>>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserCollectionsHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<List<UserCollectionDto>> Handle(GetUserCollectionsQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.UserCollections
                .AsQueryable()
                .Where(x =>
                    x.UserId == request.UserId &&
                    x.Category == request.Category)
                .ToListAsync();

            return _mapper.Map<List<UserCollectionDto>>(entities);
        }
    }
}