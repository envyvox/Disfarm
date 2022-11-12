using System;
using System.Threading.Tasks;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.World.Commands;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Hangfire.BackgroundJobs.GenerateWeather
{
    public class GenerateWeatherJob : IGenerateWeatherJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GenerateWeatherJob> _logger;
        private readonly Random _random = new();

        public GenerateWeatherJob(
            IMediator mediator,
            ILogger<GenerateWeatherJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Execute()
        {
            _logger.LogInformation(
                "Generate weather executed");

            var chance = _random.Next(1, 101);
            var state = await _mediator.Send(new GetWorldStateQuery());

            var newWeatherTomorrow = state.WeatherTomorrow == Weather.Clear
                ? chance + (state.WeatherToday == Weather.Rain ? 10 : 20) > 50
                    ? Weather.Rain
                    : Weather.Clear
                : chance + (state.WeatherToday == Weather.Clear ? 10 : 20) > 50
                    ? Weather.Clear
                    : Weather.Rain;

            await _mediator.Send(new UpdateWorldStateQuery(state with
            {
                WeatherToday = state.WeatherTomorrow,
                WeatherTomorrow = newWeatherTomorrow
            }));
        }
    }
}