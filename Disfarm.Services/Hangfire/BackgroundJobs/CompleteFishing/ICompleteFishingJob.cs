using System.Threading.Tasks;

namespace Disfarm.Services.Hangfire.BackgroundJobs.CompleteFishing
{
	public interface ICompleteFishingJob
	{
		Task Execute(ulong guildId, ulong channelId, long userId, uint cubeDrop);
	}
}