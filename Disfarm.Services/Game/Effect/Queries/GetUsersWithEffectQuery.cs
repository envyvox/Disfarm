using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Effect.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Effect.Queries
{
    public record GetUsersWithEffectQuery(Data.Enums.Effect Effect) : IRequest<List<UserEffectDto>>;

    public class GetUsersWithEffectHandler : IRequestHandler<GetUsersWithEffectQuery, List<UserEffectDto>>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUsersWithEffectHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public async Task<List<UserEffectDto>> Handle(GetUsersWithEffectQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.UserEffects
                .Include(x => x.User)
                .Where(x => x.Type == request.Effect)
                .ToListAsync();

            return _mapper.Map<List<UserEffectDto>>(entities);
        }
    }
}