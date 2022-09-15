using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Fish.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Fish.Queries
{
    public record GetUserFishQuery(long UserId, Guid FishId) : IRequest<UserFishDto>;

    public class GetUserFishHandler : IRequestHandler<GetUserFishQuery, UserFishDto>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public GetUserFishHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserFishDto> Handle(GetUserFishQuery request, CancellationToken ct)
        {
            var entity = await _db.UserFishes
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