using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Banner.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Banner.Queries
{
    public record GetUserBannersQuery(long UserId) : IRequest<List<UserBannerDto>>;

    public class GetUserBannersHandler : IRequestHandler<GetUserBannersQuery, List<UserBannerDto>>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserBannersHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<List<UserBannerDto>> Handle(GetUserBannersQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.UserBanners
                .Include(x => x.Banner)
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserBannerDto>>(entities);
        }
    }
}