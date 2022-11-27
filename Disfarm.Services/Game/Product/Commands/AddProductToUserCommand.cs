using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Product.Commands
{
	public record AddProductToUserCommand(long UserId, Guid ProductId, uint Amount) : IRequest;

	public class AddProductToUserHandler : IRequestHandler<AddProductToUserCommand>
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly ILogger<AddProductToUserHandler> _logger;

		public AddProductToUserHandler(
			IServiceScopeFactory scopeFactory,
			ILogger<AddProductToUserHandler> logger)
		{
			_scopeFactory = scopeFactory;
			_logger = logger;
		}

		public async Task<Unit> Handle(AddProductToUserCommand request, CancellationToken ct)
		{
			using var scope = _scopeFactory.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			var entity = await db.UserProducts
				.SingleOrDefaultAsync(x =>
					x.UserId == request.UserId &&
					x.ProductId == request.ProductId);

			if (entity is null)
			{
				var created = await db.CreateEntity(new UserProduct
				{
					Id = Guid.NewGuid(),
					UserId = request.UserId,
					ProductId = request.ProductId,
					Amount = request.Amount,
					CreatedAt = DateTimeOffset.UtcNow,
					UpdatedAt = DateTimeOffset.UtcNow
				});

				_logger.LogInformation(
					"Created user product entity {@Entity}",
					created);
			}
			else
			{
				entity.Amount += request.Amount;
				entity.UpdatedAt = DateTimeOffset.UtcNow;

				await db.UpdateEntity(entity);

				_logger.LogInformation(
					"Added user {UserId} product {ProductId} amount {Amount}",
					request.UserId, request.ProductId, request.Amount);
			}

			return Unit.Value;
		}
	}
}