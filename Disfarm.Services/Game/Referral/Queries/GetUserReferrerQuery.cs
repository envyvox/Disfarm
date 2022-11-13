using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.User.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Referral.Queries
{
    public record GetUserReferrerQuery(long UserId) : IRequest<UserDto>;

    public class GetUserReferrerHandler : IRequestHandler<GetUserReferrerQuery, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserReferrerHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserReferrerQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserReferrers
                .Include(x => x.Referrer)
                .Where(x => x.UserId == request.UserId)
                .Select(x => x.Referrer)
                .SingleOrDefaultAsync();

            return _mapper.Map<UserDto>(entity);
        }
    }
}