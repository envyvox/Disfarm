using System;
using System.Threading.Tasks;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.Farm.Commands;
using Disfarm.Services.Game.World.Commands;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Hangfire.BackgroundJobs.StartNewDay
{
	public class StartNewDayJob : IStartNewDayJob
	{
		private readonly IMediator _mediator;
		private readonly ILogger<StartNewDayJob> _logger;

		private readonly Random _random = new();

		public StartNewDayJob(
			IMediator mediator,
			ILogger<StartNewDayJob> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		public async Task Execute()
		{
			_logger.LogInformation(
				"Start new day job executed");


			await _mediator.Send(new MoveAllFarmsProgressCommand());

			var weatherToday = await GenerateWeather();

			await _mediator.Send(new UpdateAllFarmsStateCommand(weatherToday is Weather.Rain
				? FieldState.Watered
				: FieldState.Planted));
		}

		private async Task<Weather> GenerateWeather()
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

			return state.WeatherTomorrow;
		}
	}
}