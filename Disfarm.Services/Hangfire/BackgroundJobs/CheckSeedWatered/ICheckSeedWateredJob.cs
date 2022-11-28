using System;
using System.Threading.Tasks;

namespace Disfarm.Services.Hangfire.BackgroundJobs.CheckSeedWatered
{
    public interface ICheckSeedWateredJob
    {
        Task Execute(Guid userFarmId, string completeSeedGrowthJobId);
    }
}