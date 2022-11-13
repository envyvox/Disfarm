using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.User.Commands;
using Disfarm.Services.Game.User.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.User.Queries
{
    public record GetUserQuery(long UserId) : IRequest<UserDto>;

    public class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            IMediator mediator)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.Users.SingleOrDefaultAsync(x => x.Id == request.UserId);

            if (entity is null)
            {
                return await _mediator.Send(new CreateUserCommand(request.UserId));
            }

            return _mapper.Map<UserDto>(entity);
        }
    }
}