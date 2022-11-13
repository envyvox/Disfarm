using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Seed.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Seed.Queries
{
    public record GetRandomSeedQuery : IRequest<SeedDto>;

    public class GetRandomSeedHandler : IRequestHandler<GetRandomSeedQuery, SeedDto>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetRandomSeedHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<SeedDto> Handle(GetRandomSeedQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.Seeds
                .OrderByRandom()
                .Include(x => x.Crop)
                .FirstOrDefaultAsync();

            return _mapper.Map<SeedDto>(entity);
        }
    }
}