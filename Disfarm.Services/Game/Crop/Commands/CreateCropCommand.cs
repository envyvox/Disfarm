using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Crop.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Crop.Commands
{
    public record CreateCropCommand(string Name, uint Price, Guid SeedId) : IRequest<CropDto>;

    public class CreateCropHandler : IRequestHandler<CreateCropCommand, CropDto>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCropHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public CreateCropHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            ILogger<CreateCropHandler> logger)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CropDto> Handle(CreateCropCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var exist = await db.Crops
                .AnyAsync(x => x.Name == request.Name);

            if (exist)
            {
                throw new Exception(
                    $"crop with name {request.Name} already exist");
            }

            var created = await db.CreateEntity(new Data.Entities.Crop
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                SeedId = request.SeedId,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created crop entity {@Entity}",
                created);

            return _mapper.Map<CropDto>(created);
        }
    }
}