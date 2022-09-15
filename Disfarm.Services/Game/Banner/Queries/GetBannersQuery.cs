using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Banner.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Banner.Queries
{
    public record GetBannersQuery : IRequest<List<BannerDto>>;

    public class GetBannersHandler : IRequestHandler<GetBannersQuery, List<BannerDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetBannersHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<List<BannerDto>> Handle(GetBannersQuery request, CancellationToken ct)
        {
            var entities = await _db.Banners
                .AsQueryable()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<BannerDto>>(entities);
        }
    }
}