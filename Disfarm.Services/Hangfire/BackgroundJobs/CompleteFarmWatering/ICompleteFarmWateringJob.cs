using System.Threading.Tasks;

namespace Disfarm.Services.Hangfire.BackgroundJobs.CompleteFarmWatering
{
    public interface ICompleteFarmWateringJob
    {
        Task Execute(ulong guildId, long userId, uint farmCount);
    }
}