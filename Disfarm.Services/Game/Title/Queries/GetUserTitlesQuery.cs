using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Title.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Title.Queries
{
    public record GetUserTitlesQuery(long UserId) : IRequest<List<UserTitleDto>>;

    public class GetUserTitlesHandler : IRequestHandler<GetUserTitlesQuery, List<UserTitleDto>>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserTitlesHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public async Task<List<UserTitleDto>> Handle(GetUserTitlesQuery request,
            CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.UserTitles
                .AsQueryable()
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            return _mapper.Map<List<UserTitleDto>>(entities);
        }
    }
}