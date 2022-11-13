using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Fish.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Fish.Queries
{
    public record GetUserFishQuery(long UserId, Guid FishId) : IRequest<UserFishDto>;

    public class GetUserFishHandler : IRequestHandler<GetUserFishQuery, UserFishDto>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public GetUserFishHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<UserFishDto> Handle(GetUserFishQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserFishes
                .Include(x => x.Fish)
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.FishId == request.FishId);

            return entity is null
                ? new UserFishDto(Guid.Empty, null, 0, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
                : _mapper.Map<UserFishDto>(entity);
        }
    }
}