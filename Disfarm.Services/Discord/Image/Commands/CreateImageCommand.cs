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
            var exist = await _db.Images
                .AnyAsync(x =>
                    x.Type == request.Type &&
                    x.Language == request.Language);

            if (exist)
            {
                throw new Exception(
                    $"image with type {request.Type.ToString()} and language {request.Language.ToString()} already exist");
            }

            var created = await _db.CreateEntity(new Data.Entities.Image
            {
                Id = Guid.NewGuid(),
                Type = request.Type,
                Language = request.Language,
                Url = request.Url
            });

            _logger.LogInformation(
                "Created image entity {@Entity}",
                created);

            return Unit.Value;
        }
    }
}