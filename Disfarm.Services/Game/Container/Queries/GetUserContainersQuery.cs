using System.Collections.Generic;
using System.Linq;
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
    public record GetUserContainersQuery(long UserId) : IRequest<Dictionary<Data.Enums.Container, UserContainerDto>>;

    public class GetUserContainersHandler
        : IRequestHandler<GetUserContainersQuery, Dictionary<Data.Enums.Container, UserContainerDto>>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetUserContainersHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<Dictionary<Data.Enums.Container, UserContainerDto>> Handle(GetUserContainersQuery request,
            CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entities = await db.UserContainers
                .AsQueryable()
                .Where(x => x.UserId == request.UserId)
                .ToDictionaryAsync(x => x.Type);

            return _mapper.Map<Dictionary<Data.Enums.Container, UserContainerDto>>(entities);
        }
    }
}