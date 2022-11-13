using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Localization.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Localization.Commands
{
    public record UpdateLocalizationCommand(
            Guid Id,
            string Single,
            string Double,
            string Multiply)
        : IRequest<LocalizationDto>;

    public class UpdateLocalizationHandler : IRequestHandler<UpdateLocalizationCommand, LocalizationDto>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public UpdateLocalizationHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<LocalizationDto> Handle(UpdateLocalizationCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.Localizations
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception(
                    $"localization {request.Id} not found");
            }

            entity.Single = request.Single;
            entity.Double = request.Double;
            entity.Multiply = request.Multiply;

            await db.UpdateEntity(entity);

            return _mapper.Map<LocalizationDto>(entity);
        }
    }
}