using System;
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
    public record GetUserActiveBannerQuery(long UserId) : IRequest<BannerDto>;

    public class GetUserActiveBannerHandler : IRequestHandler<GetUserActiveBannerQuery, BannerDto>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserActiveBannerHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<BannerDto> Handle(GetUserActiveBannerQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserBanners
                .Include(x => x.Banner)
                .Where(x => x.UserId == request.UserId && x.IsActive)
                .Select(x => x.Banner)
                .SingleOrDefaultAsync();

            if (entity is null)
            {
                throw new Exception(
                    $"user {request.UserId} doesnt have active banner.");
            }

            return _mapper.Map<BannerDto>(entity);
        }
    }
}