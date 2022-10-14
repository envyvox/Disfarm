using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Effect.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Effect.Queries
{
    public record GetUsersWithEffectQuery(Data.Enums.Effect Effect) : IRequest<List<UserEffectDto>>;

    public class GetUsersWithEffectHandler : IRequestHandler<GetUsersWithEffectQuery, List<UserEffectDto>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUsersWithEffectHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _mapper = mapper;
            _db = new AppDbContext(options);
        }

        public async Task<List<UserEffectDto>> Handle(GetUsersWithEffectQuery request, CancellationToken ct)
        {
            var entities = await _db.UserEffects
                .Include(x => x.User)
                .Where(x => x.Type == request.Effect)
                .ToListAsync();

            return _mapper.Map<List<UserEffectDto>>(entities);
        }
    }
}