using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Client;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Discord.Embed
{
    public record SendEmbedToUserCommand(
            ulong GuildId,
            ulong ChannelId,
            ulong UserId,
            EmbedBuilder EmbedBuilder,
            ComponentBuilder ComponentBuilder = null,
            string Text = "",
            bool ShowLinkButton = true)
        : IRequest;

    public class SendEmbedToUserHandler : IRequestHandler<SendEmbedToUserCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SendEmbedToUserHandler> _logger;
        private readonly IDiscordClientService _discordClientService;

        public SendEmbedToUserHandler(
            IMediator mediator,
            ILogger<SendEmbedToUserHandler> logger,
            IDiscordClientService discordClientService)
        {
            _mediator = mediator;
            _logger = logger;
            _discordClientService = discordClientService;
        }

        public async Task<Unit> Handle(SendEmbedToUserCommand request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.UserId));
            var client = await _discordClientService.GetSocketClient();
            var socketUser = await client.GetUserAsync(request.UserId);
            var builder = request.ComponentBuilder ?? new ComponentBuilder();

            if (request.ShowLinkButton)
            {
                builder.WithButton(
                    label: Response.ComponentOpenExecutedChannel.Parse(user.Language),
                    style: ButtonStyle.Link,
                    url: $"https://discord.com/channels/{request.GuildId}/{request.ChannelId}");
            }

            try
            {
                await socketUser.SendMessageAsync(
                    text: request.Text,
                    embed: request.EmbedBuilder.WithUserColor(user.CommandColor).Build(),
                    components: builder.Build());

                _logger.LogInformation(
                    "Sended a direct message to user {UserId}",
                    request.UserId);
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    "Can't send message to user {UserId}",
                    request.UserId);
            }

            return Unit.Value;
        }
    }
}