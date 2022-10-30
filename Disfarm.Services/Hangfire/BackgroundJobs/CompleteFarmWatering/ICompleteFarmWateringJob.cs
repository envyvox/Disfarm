using System.Threading.Tasks;

namespace Disfarm.Services.Hangfire.BackgroundJobs.CompleteFarmWatering
{
	public interface ICompleteFarmWateringJob
	{
		Task Execute(ulong guildId, ulong channelId, long userId, uint farmCount);
	}
}