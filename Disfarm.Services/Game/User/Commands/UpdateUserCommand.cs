using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.User.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.User.Commands
{
    public record UpdateUserCommand(UserDto UpdatedUser) : IRequest;

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly ILogger<UpdateUserHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public UpdateUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<UpdateUserHandler> logger,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var updated = await db.UpdateEntity(_mapper.Map<Data.Entities.User.User>(request.UpdatedUser with
            {
                UpdatedAt = DateTimeOffset.UtcNow
            }));

            _logger.LogInformation(
                "Updated user entity {@Entity}",
                updated);

            return Unit.Value;
        }
    }
}