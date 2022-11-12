using System.Threading.Tasks;

namespace Disfarm.Services.Hangfire.BackgroundJobs.GenerateWeather
{
    public interface IGenerateWeatherJob
    {
        Task Execute();
    }
}