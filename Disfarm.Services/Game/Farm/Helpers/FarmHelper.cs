using System;
using System.Collections.Generic;
using System.Linq;
using Disfarm.Data.Enums;
using Disfarm.Services.Hangfire.BackgroundJobs.CheckSeedWatered;
using Disfarm.Services.Hangfire.BackgroundJobs.CompleteSeedGrowth;
using Hangfire;

namespace Disfarm.Services.Game.Farm.Helpers
{
    public static class FarmHelper
    {
        /// <summary>
        /// Schedule hangfire background job for completing seed growth
        /// and if delay is more than 24 hours - also scheduling check seed watered job
        /// </summary>
        public static void ScheduleBackgroundJobs(long userId, Guid userFarmId, TimeSpan delay)
        {
            var completeSeedGrowthJobId = BackgroundJob.Schedule<ICompleteSeedGrowthJob>(x =>
                    x.Execute(userId, userFarmId),
                delay);

            if (delay > TimeSpan.FromHours(24))
            {
                BackgroundJob.Schedule<ICheckSeedWateredJob>(x =>
                        x.Execute(userId, userFarmId, completeSeedGrowthJobId),
                    TimeSpan.FromHours(24));
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