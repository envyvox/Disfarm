using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Discord.Image.Commands
{
    public record CreateImageCommand(Data.Enums.Image Type, Language Language, string Url) : IRequest;

    public class CreateImageHandler : IRequestHandler<CreateImageCommand>
    {
        private readonly ILogger<CreateImageHandler> _logger;
        private readonly AppDbContext _db;

        public CreateImageHandler(
            DbContextOptions options,
            ILogger<CreateImageHandler> logger)
        {
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateImageCommand request, CancellationToken ct)
        {
            var entity = await _db.Images.FirstOrDefaultAsync(x =>
                x.Type == request.Type &&
                x.Language == request.Language);

            if (entity is null)
            {
                var created = await _db.CreateEntity(new Data.Entities.Image
                {
                    Id = Guid.NewGuid(),
                    Type = request.Type,
                    Language = request.Language,
                    Url = request.Url,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created image entity {@Entity}",
                    created);
            }
            else
            {
                entity.Url = request.Url;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Updated image entity {Type}, {Language}, {Url}",
                    request.Type, request.Language, request.Url);
            }

            return Unit.Value;
        }
    }
}