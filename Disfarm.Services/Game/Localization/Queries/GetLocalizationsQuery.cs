using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Localization.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Localization.Queries
{
    public record GetLocalizationsQuery : IRequest<List<LocalizationDto>>;

    public class GetLocalizationsHandler : IRequestHandler<GetLocalizationsQuery, List<LocalizationDto>>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetLocalizationsHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<List<LocalizationDto>> Handle(GetLocalizationsQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.Localizations
                .AsQueryable()
                .OrderBy(x => x.Category)
                .ToListAsync();

            return _mapper.Map<List<LocalizationDto>>(entities);
        }
    }
}