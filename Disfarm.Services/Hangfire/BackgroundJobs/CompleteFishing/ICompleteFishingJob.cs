using System.Threading.Tasks;

namespace Disfarm.Services.Hangfire.BackgroundJobs.CompleteFishing
{
    public interface ICompleteFishingJob
    {
        Task Execute(long guildId, long userId, uint cubeDrop);
    }
}