using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Fish.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Fish.Queries
{
    public record GetRandomFishWithRarityQuery(FishRarity Rarity) : IRequest<FishDto>;

    public class GetRandomFishWithRarityHandler : IRequestHandler<GetRandomFishWithRarityQuery, FishDto>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetRandomFishWithRarityHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<FishDto> Handle(GetRandomFishWithRarityQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.Fishes
                .OrderByRandom()
                .FirstOrDefaultAsync(x => x.Rarity == request.Rarity);

            if (entity is null)
            {
                throw new Exception(
                    $"fish with rarity {request.Rarity.ToString()} not found");
            }

            return _mapper.Map<FishDto>(entity);
        }
    }
}