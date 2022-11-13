using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Crop.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Crop.Queries
{
    public record GetUserCropsQuery(long UserId) : IRequest<List<UserCropDto>>;

    public class GetUserCropsHandler : IRequestHandler<GetUserCropsQuery, List<UserCropDto>>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserCropsHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<List<UserCropDto>> Handle(GetUserCropsQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.UserCrops
                .Include(x => x.Crop).ThenInclude(x => x.Seed)
                .Where(x =>
                    x.UserId == request.UserId &&
                    x.Amount > 0)
                .ToListAsync();

            return _mapper.Map<List<UserCropDto>>(entities);
        }
    }
}