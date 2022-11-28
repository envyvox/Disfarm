using System;
using System.Linq;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Hangfire.BackgroundJobs.UpdateFarmState
{
	public class UpdateFieldStateJob : IUpdateFieldStateJob
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly ILogger<UpdateFieldStateJob> _logger;

		public UpdateFieldStateJob(
			IServiceScopeFactory scopeFactory,
			ILogger<UpdateFieldStateJob> logger)
		{
			_scopeFactory = scopeFactory;
			_logger = logger;
		}

		public async Task Execute(Guid userFarmId, FieldState state)
		{
			using var scope = _scopeFactory.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			var entity = await db.UserFarms.SingleOrDefaultAsync(x => x.Id == userFarmId);

			if (entity is null)
			{
				throw new Exception(
					$"User farm with id {userFarmId} not found");
			}

			entity.State = state;
			entity.UpdatedAt = DateTimeOffset.UtcNow;

			await db.UpdateEntity(entity);

			_logger.LogInformation(
				"Updated user {UserId} farm {Number} state to {State}",
				entity.UserId, entity.Number, entity.State);
		}
	}
}