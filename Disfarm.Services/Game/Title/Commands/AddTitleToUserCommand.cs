using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Title.Commands
{
    public record AddTitleToUserCommand(long UserId, Data.Enums.Title Type) : IRequest;

    public class AddTitleToUserHandler : IRequestHandler<AddTitleToUserCommand>
    {
        private readonly ILogger<AddTitleToUserHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public AddTitleToUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<AddTitleToUserHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(AddTitleToUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var exist = await db.UserTitles
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (exist)
            {
                throw new Exception(
                    $"user {request.UserId} already have title {request.Type.ToString()}");
            }

            var created = await db.CreateEntity(new UserTitle
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Type = request.Type,
                CreatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created user title entity {@Entity}",
                created);

            return Unit.Value;
        }
    }
}