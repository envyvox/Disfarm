using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.User.Commands;
using Disfarm.Services.Game.User.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.User.Queries
{
    public record GetUserQuery(long UserId) : IRequest<UserDto>;

    public class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public GetUserHandler(
            DbContextOptions options,
            IMapper mapper,
            IMediator mediator)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken ct)
        {
            var entity = await _db.Users.SingleOrDefaultAsync(x => x.Id == request.UserId);

            if (entity is null)
            {
                return await _mediator.Send(new CreateUserCommand(request.UserId));
            }

            return _mapper.Map<UserDto>(entity);
        }
    }
}