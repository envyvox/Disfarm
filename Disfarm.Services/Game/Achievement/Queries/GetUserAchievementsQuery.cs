using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.Achievement.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Achievement.Queries
{
    public record GetUserAchievementsQuery(
            long UserId,
            AchievementCategory Category)
        : IRequest<List<UserAchievementDto>>;

    public class GetUserAchievementsHandler : IRequestHandler<GetUserAchievementsQuery, List<UserAchievementDto>>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserAchievementsHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<List<UserAchievementDto>> Handle(GetUserAchievementsQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.UserAchievements
                .Include(x => x.Achievement)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            entities = entities.Where(x => x.Achievement.Type.Category() == request.Category).ToList();

            return _mapper.Map<List<UserAchievementDto>>(entities);
        }
    }
}