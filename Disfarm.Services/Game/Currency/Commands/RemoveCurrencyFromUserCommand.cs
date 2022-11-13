using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Currency.Commands
{
    public record RemoveCurrencyFromUserCommand(
            long UserId,
            Data.Enums.Currency Type,
            uint Amount)
        : IRequest;

    public class RemoveCurrencyFromUserHandler : IRequestHandler<RemoveCurrencyFromUserCommand>
    {
        private readonly ILogger<RemoveCurrencyFromUserHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public RemoveCurrencyFromUserHandler(
            IServiceScopeFactory scopeFactory,
            ILogger<RemoveCurrencyFromUserHandler> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<Unit> Handle(RemoveCurrencyFromUserCommand request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.UserCurrencies
                .SingleOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Type == request.Type);

            if (entity is null)
            {
                throw new Exception(
                    $"user {request.UserId} doesnt have currency {request.Type.ToString()}");
            }

            entity.Amount -= request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await db.UpdateEntity(entity);

            _logger.LogInformation(
                "Removed from user {UserId} currency {Currency} amount {Amount}",
                request.UserId, request.Type.ToString(), request.Amount);

            return Unit.Value;
        }
    }
}