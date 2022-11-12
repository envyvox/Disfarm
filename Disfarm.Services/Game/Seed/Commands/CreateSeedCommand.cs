using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Seed.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Seed.Commands
{
    public record CreateSeedCommand(
            string Name,
            Season Season,
            TimeSpan Growth,
            TimeSpan? ReGrowth,
            bool IsMultiply,
            uint Price)
        : IRequest<SeedDto>;

    public class CreateSeedHandler : IRequestHandler<CreateSeedCommand, SeedDto>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSeedHandler> _logger;
        private readonly AppDbContext _db;

        public CreateSeedHandler(
            DbContextOptions options,
            IMapper mapper,
            ILogger<CreateSeedHandler> logger)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SeedDto> Handle(CreateSeedCommand request, CancellationToken ct)
        {
            var exist = await _db.Seeds
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception(
                    $"seed with name {request.Name} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Seed
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Season = request.Season,
                Growth = request.Growth,
                ReGrowth = request.ReGrowth,
                IsMultiply = request.IsMultiply,
                Price = request.Price,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created seed entity {@Entity}",
                created);

            return _mapper.Map<SeedDto>(created);
        }
    }
}