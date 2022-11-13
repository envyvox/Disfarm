using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Fish.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Fish.Queries
{
    public record GetUserFishesQuery(long UserId) : IRequest<List<UserFishDto>>;

    public class GetUserFishesHandler : IRequestHandler<GetUserFishesQuery, List<UserFishDto>>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public GetUserFishesHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<List<UserFishDto>> Handle(GetUserFishesQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.UserFishes
                .Include(x => x.Fish)
                .Where(x =>
                    x.UserId == request.UserId &&
                    x.Amount > 0)
                .ToListAsync();

            return _mapper.Map<List<UserFishDto>>(entities);
        }
    }
}