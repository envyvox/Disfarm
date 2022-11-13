using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Entities;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.World.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.World.Commands
{
    public record UpdateWorldStateQuery(WorldStateDto WorldState) : IRequest;

    public class UpdateWorldStateHandler : IRequestHandler<UpdateWorldStateQuery>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateWorldStateHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public UpdateWorldStateHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            ILogger<UpdateWorldStateHandler> logger)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateWorldStateQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var updated = await db.UpdateEntity(_mapper.Map<WorldState>(request.WorldState with
            {
                UpdatedAt = DateTimeOffset.UtcNow
            }));

            _logger.LogInformation(
                "Updated world state entity {@Entity}",
                updated);

            return Unit.Value;
        }
    }
}