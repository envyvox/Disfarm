using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Localization.Commands
{
    public record CreateLocalizationCommand(
            LocalizationCategory Category,
            string Name,
            Language Language,
            string Single,
            string Double,
            string Multiply)
        : IRequest;

    public class CreateLocalizationHandler : IRequestHandler<CreateLocalizationCommand>
    {
        private readonly ILogger<CreateLocalizationHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public CreateLocalizationHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<CreateLocalizationHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(CreateLocalizationCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var exist = await db.Localizations
                .AnyAsync(x =>
                    x.Category == request.Category &&
                    x.Name == request.Name &&
                    x.Language == request.Language);

            if (exist)
            {
                throw new Exception(
                    $"localization with category {request.Category.ToString()}, name {request.Name} and language {request.Language.ToString()} already exist");
            }

            var created = await db.CreateEntity(new Data.Entities.Localization
            {
                Id = Guid.NewGuid(),
                Category = request.Category,
                Name = request.Name,
                Language = request.Language,
                Single = request.Single,
                Double = request.Double,
                Multiply = request.Multiply
            });

            _logger.LogInformation(
                "Created localization entity {@Entity}",
                created);

            return Unit.Value;
        }
    }
}