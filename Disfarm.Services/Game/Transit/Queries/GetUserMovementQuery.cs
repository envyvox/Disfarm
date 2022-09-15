using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Transit.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Transit.Queries
{
    public record GetUserMovementQuery(long UserId) : IRequest<UserMovementDto>;

    public class GetUserMovementHandler : IRequestHandler<GetUserMovementQuery, UserMovementDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetUserMovementHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<UserMovementDto> Handle(GetUserMovementQuery request, CancellationToken ct)
        {
            var entity = await _db.UserMovements
                .SingleOrDefaultAsync(x => x.UserId == request.UserId);

            if (entity is null)
            {
                throw new Exception($"user {request.UserId} doesnt have movement entity");
            }

            return _mapper.Map<UserMovementDto>(entity);
        }
    }
}