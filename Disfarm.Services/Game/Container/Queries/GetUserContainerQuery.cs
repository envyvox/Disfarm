using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Container.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Container.Queries
{
    public record GetUserContainerQuery(long UserId, Data.Enums.Container Type) : IRequest<UserContainerDto>;

    public class GetUserContainerHandler : IRequestHandler<GetUserContainerQuery, UserContainerDto>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserContainerHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<UserContainerDto> Handle(GetUserContainerQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserContainers
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            return entity is null
                ? new UserContainerDto(Guid.Empty, request.Type, 0, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
                : _mapper.Map<UserContainerDto>(entity);
        }
    }
}