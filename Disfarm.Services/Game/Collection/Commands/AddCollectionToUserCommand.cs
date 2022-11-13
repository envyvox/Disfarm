using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Collection.Commands
{
    public record AddCollectionToUserCommand(long UserId, CollectionCategory Category, Guid ItemId) : IRequest;

    public class AddCollectionToUserHandler : IRequestHandler<AddCollectionToUserCommand>
    {
        private readonly ILogger<AddCollectionToUserHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _scopeFactory;

        public AddCollectionToUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<AddCollectionToUserHandler> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(AddCollectionToUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var exist = await db.UserCollections
                .AnyAsync(x =>
                    x.UserId == request.UserId &&
                    x.Category == request.Category &&
                    x.ItemId == request.ItemId);

            if (exist) return Unit.Value;

            var created = await db.CreateEntity(new UserCollection
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Category = request.Category,
                ItemId = request.ItemId,
                CreatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created user collection entity {@Entity}",
                created);

            // return await _mediator.Send(new CheckAchievementInUserCommand(request.UserId, request.Category switch
            // {
            //     CollectionCategory.Crop => Data.Enums.Achievement.CompleteCollectionCrop,
            //     CollectionCategory.Fish => Data.Enums.Achievement.CompleteCollectionFish,
            //     _ => throw new ArgumentOutOfRangeException()
            // }));

            return Unit.Value;
        }
    }
}