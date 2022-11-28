using System;
using System.Threading.Tasks;

namespace Disfarm.Services.Hangfire.BackgroundJobs.CompleteSeedGrowth
{
    public interface ICompleteSeedGrowthJob
    {
        Task Execute(Guid userFarmId);
    }
}