using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Fish.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Fish.Commands
{
    public record CreateFishCommand(
            string Name,
            FishRarity Rarity,
            List<Season> CatchSeasons,
            Weather CatchWeather,
            TimesDayType CatchTimesDay,
            uint Price)
        : IRequest<FishDto>;

    public class CreateFishHandler : IRequestHandler<CreateFishCommand, FishDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateFishHandler> _logger;

        public CreateFishHandler(
            DbContextOptions options,
            IMapper mapper,
            ILogger<CreateFishHandler> logger)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FishDto> Handle(CreateFishCommand request, CancellationToken ct)
        {
            var exist = await _db.Fishes
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception(
                    $"fish with name {request.Name} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Fish
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Rarity = request.Rarity,
                CatchWeather = request.CatchWeather,
                CatchTimesDay = request.CatchTimesDay,
                CatchSeasons = request.CatchSeasons,
                Price = request.Price,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created fish entity {@Entity}",
                created);

            return _mapper.Map<FishDto>(created);
        }
    }
}