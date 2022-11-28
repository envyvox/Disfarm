using System;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Hangfire.BackgroundJobs.CheckSeedWatered
{
	public class CheckSeedWateredJob : ICheckSeedWateredJob
	{
		private readonly ILogger<CheckSeedWateredJob> _logger;
		private readonly IServiceScopeFactory _scopeFactory;

		public CheckSeedWateredJob(
			IServiceScopeFactory scopeFactory,
			ILogger<CheckSeedWateredJob> logger)
		{
			_scopeFactory = scopeFactory;
			_logger = logger;
		}

		public async Task Execute(Guid userFarmId, string completeSeedGrowthJobId)
		{
			using var scope = _scopeFactory.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			var entity = await db.UserFarms.SingleOrDefaultAsync(x => x.Id == userFarmId);

			switch (entity.State)
			{
				case FieldState.Empty:
				{
					_logger.LogInformation(
						"Checked seed watered for user {UserId} farm {Number} but state is already empty",
						entity.UserId, entity.Number);
					break;
				}
				case FieldState.Completed:
				{
					_logger.LogInformation(
						"Checked seed watered for user {UserId} farm {Number} but state is already completed",
						entity.UserId, entity.Number);
					break;
				}
				case FieldState.Planted:
				{
					BackgroundJob.Delete(completeSeedGrowthJobId);

					entity.BeenGrowingFor =
						entity.BeenGrowingFor?.Add(TimeSpan.FromHours(16)) ?? TimeSpan.FromHours(16);

					await db.UpdateEntity(entity);

					_logger.LogInformation(
						"Checked seed watered for user {UserId} farm {Number} and state is planted, " +
						"deleted complete seed growth job and added 16h growth time to entity",
						entity.UserId, entity.Number);
					break;
				}
				case FieldState.Watered:
				{
					_logger.LogInformation(
						"Check seed watered for user {UserId} farm {Number} and state is watered",
						entity.UserId, entity.Number);

					BackgroundJob.Schedule<ICheckSeedWateredJob>(x =>
							x.Execute(userFarmId, completeSeedGrowthJobId),
						TimeSpan.FromHours(16));
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}