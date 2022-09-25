using System.Threading.Tasks;

namespace Disfarm.Services.Hangfire.BackgroundJobs.CompleteFishing
{
    public interface ICompleteFishingJob
    {
        Task Execute(ulong guildId, long userId, uint cubeDrop);
    }
}