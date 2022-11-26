using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.Interactions;
using Disfarm.Services.Discord.Emote.Commands;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Hangfire.BackgroundJobs.GenerateWeather;
using Disfarm.Services.Hangfire.BackgroundJobs.ResetDailyRewards;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disfarm.Services.Discord.Client.Events
{
    public record OnReady(InteractionService InteractionService) : IRequest;

    public class OnReadyHandler : IRequestHandler<OnReady>
    {
        private readonly ILogger<OnReadyHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly IHostEnvironment _environment;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly DiscordClientOptions _options;

        public OnReadyHandler(
            IOptions<DiscordClientOptions> options,
            ILogger<OnReadyHandler> logger,
            IMediator mediator,
            IHostApplicationLifetime lifetime,
            IHostEnvironment environment,
            TimeZoneInfo timeZoneInfo)
        {
            _logger = logger;
            _mediator = mediator;
            _lifetime = lifetime;
            _environment = environment;
            _timeZoneInfo = timeZoneInfo;
            _options = options.Value;
        }

        public async Task<Unit> Handle(OnReady request, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new SyncEmotesCommand());

                RecurringJob.AddOrUpdate<IResetDailyRewardJob>("reset-daily-reward",
                    x => x.Execute(),
                    Cron.Weekly, _timeZoneInfo);

                RecurringJob.AddOrUpdate<IGenerateWeatherJob>("generate-weather",
                    x => x.Execute(),
                    Cron.Daily, _timeZoneInfo);

                if (_environment.IsDevelopment())
                {
                    _logger.LogInformation(
                        "Environment is development. Registering commands to guild {GuildId}",
                        _options.BetaGuildId);

                    await request.InteractionService.RegisterCommandsToGuildAsync(_options.BetaGuildId);
                }
                else
                {
                    _logger.LogInformation(
                        "Environment is production. Registering commands globally");

                    await request.InteractionService.RegisterCommandsGloballyAsync();
                }

                _logger.LogInformation(
                    "Bot started");
            }
            catch (Exception e)
            {
                _logger.LogCritical(e,
                    "Unable to startup the bot. Application will now exit");

                _lifetime.StopApplication();
            }

            return new Unit();
        }
    }
}