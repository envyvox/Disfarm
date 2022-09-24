using System.Threading.Tasks;

namespace Disfarm.Services.Hangfire.BackgroundJobs.StartNewDay
{
    public interface IStartNewDayJob
    {
        Task Execute();
    }
}