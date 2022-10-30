using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Disfarm.Services.Discord.Client.Events;
using Disfarm.Services.Discord.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Disfarm.Services.Discord.Client
{
	public class DiscordClientService : IDiscordClientService
	{
		private readonly DiscordClientOptions _options;
		private readonly IServiceProvider _serviceProvider;
		private readonly IMediator _mediator;
		private DiscordSocketClient _socketClient;
		private InteractionService _interactionService;

		public DiscordClientService(
			IOptions<DiscordClientOptions> options,
			IServiceProvider serviceProvider,
			IMediator mediator)
		{
			_options = options.Value;
			_serviceProvider = serviceProvider;
			_mediator = mediator;
		}

		public async Task Start()
		{
			_socketClient = new DiscordSocketClient(new DiscordSocketConfig
			{
				LogLevel = LogSeverity.Info,
				MessageCacheSize = 100,
				AlwaysDownloadUsers = true,
				GatewayIntents =
					GatewayIntents.Guilds |
					GatewayIntents.GuildMembers |
					GatewayIntents.GuildMessageReactions |
					GatewayIntents.GuildMessages |
					GatewayIntents.GuildVoiceStates |
					GatewayIntents.GuildScheduledEvents
			});
			_interactionService = new InteractionService(_socketClient.Rest, new InteractionServiceConfig
			{
				UseCompiledLambda = true
			});

			await _serviceProvider.GetRequiredService<CommandHandler>()
				.InitializeAsync(_socketClient, _interactionService, _serviceProvider);

			await _socketClient.LoginAsync(TokenType.Bot, _options.Token);
			await _socketClient.StartAsync();

			_socketClient.Ready += ClientOnReady;
		}

		private async Task ClientOnReady()
		{
			await _mediator.Send(new OnReady(_interactionService));
		}

		public async Task<DiscordSocketClient> GetSocketClient()
		{
			return await Task.FromResult(_socketClient);
		}
	}
}