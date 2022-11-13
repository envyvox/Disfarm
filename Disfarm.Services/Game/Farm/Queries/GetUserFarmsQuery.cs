using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Farm.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Farm.Queries
{
    public record GetUserFarmsQuery(long UserId) : IRequest<List<UserFarmDto>>;

    public class GetUserFarmsHandler : IRequestHandler<GetUserFarmsQuery, List<UserFarmDto>>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserFarmsHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<List<UserFarmDto>> Handle(GetUserFarmsQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.UserFarms
                .Include(x => x.Seed)
                .ThenInclude(x => x.Crop)
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Number)
                .ToListAsync();

            return _mapper.Map<List<UserFarmDto>>(entities);
        }
    }
}