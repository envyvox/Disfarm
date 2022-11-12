using System;
using System.Threading.Tasks;

namespace Disfarm.Services.Hangfire.BackgroundJobs.CheckSeedWatered
{
    public interface ICheckSeedWateredJob
    {
        Task Execute(long userId, Guid userFarmId, string completeSeedGrowthJobId);
    }
}