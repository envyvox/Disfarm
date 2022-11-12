using System;
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
        /// <param name="userId"></param>
        /// <param name="userFarmId"></param>
        /// <param name="delay"></param>
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
    }
}