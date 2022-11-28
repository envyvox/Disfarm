using System;
using System.Collections.Generic;
using System.Linq;
using Disfarm.Data.Enums;
using Disfarm.Services.Hangfire.BackgroundJobs.CheckSeedWatered;
using Disfarm.Services.Hangfire.BackgroundJobs.CompleteSeedGrowth;
using Disfarm.Services.Hangfire.BackgroundJobs.UpdateFarmState;
using Hangfire;

namespace Disfarm.Services.Game.Farm.Helpers
{
	public static class FarmHelper
	{
		/// <summary>
		/// Schedule hangfire background jobs
		/// </summary>
		public static void ScheduleBackgroundJobs(Guid userFarmId, TimeSpan completionTime)
		{
			var completeSeedGrowthJobId = BackgroundJob.Schedule<ICompleteSeedGrowthJob>(x =>
					x.Execute(userFarmId),
				completionTime);

			BackgroundJob.Schedule<IUpdateFieldStateJob>(x =>
					x.Execute(userFarmId, FieldState.Planted),
				TimeSpan.FromHours(12));

			if (completionTime > TimeSpan.FromHours(16))
			{
				BackgroundJob.Schedule<ICheckSeedWateredJob>(x =>
						x.Execute(userFarmId, completeSeedGrowthJobId),
					TimeSpan.FromHours(16));
			}
		}

		/// <summary>
		/// Subtracts from the default time % acceleration from buildings
		/// </summary>
		public static TimeSpan CompletionTimeAfterBuildingsSpeedBonus(TimeSpan defaultTime,
			IEnumerable<Data.Enums.Building> buildings)
		{
			var speedBonusPercent = buildings.Aggregate<Data.Enums.Building, uint>(0,
				(current, building) => current + building.SpeedBonusPercent() ?? 0);

			return TimeSpan.FromMinutes(defaultTime.Duration().TotalMinutes -
			                            defaultTime.Duration().TotalMinutes * speedBonusPercent / 100);
		}
	}
}