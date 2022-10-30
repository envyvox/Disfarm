using System.Threading.Tasks;

namespace Disfarm.Services.Hangfire.BackgroundJobs.ResetDailyRewards
{
	public interface IResetDailyRewardJob
	{
		Task Execute();
	}
}