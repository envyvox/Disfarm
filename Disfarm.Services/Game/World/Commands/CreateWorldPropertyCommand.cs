using System;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.World.Commands
{
	public record CreateWorldPropertyCommand(WorldProperty Type, uint Value) : IRequest;

	public class CreateWorldPropertyHandler : IRequestHandler<CreateWorldPropertyCommand>
	{
		private readonly ILogger<CreateWorldPropertyHandler> _logger;
		private readonly AppDbContext _db;

		public CreateWorldPropertyHandler(
			DbContextOptions options,
			ILogger<CreateWorldPropertyHandler> logger)
		{
			_logger = logger;
			_db = new AppDbContext(options);
		}

		public async Task<Unit> Handle(CreateWorldPropertyCommand request, CancellationToken ct)
		{
			var exist = await _db.WorldProperties
				.AnyAsync(x => x.Type == request.Type);

			if (exist)
			{
				throw new Exception(
					$"world property {request.Type.ToString()} already exist");
			}

			var created = await _db.CreateEntity(new Data.Entities.WorldProperty
			{
				Type = request.Type,
				Value = request.Value,
				CreatedAt = DateTimeOffset.UtcNow,
				UpdatedAt = DateTimeOffset.UtcNow
			});

			_logger.LogInformation(
				"Created world property entity {@Entity}",
				created);

			return Unit.Value;
		}
	}
}