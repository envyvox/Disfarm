using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Disfarm.Services.Discord.Guild.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Discord.Embed
{
    public record SendEmbedToUserCommand(
            ulong GuildId,
            ulong UserId,
            EmbedBuilder EmbedBuilder,
            MessageComponent Components = null,
            string Text = "")
        : IRequest;

    public class SendEmbedToUserHandler : IRequestHandler<SendEmbedToUserCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SendEmbedToUserHandler> _logger;

        public SendEmbedToUserHandler(
            IMediator mediator,
            ILogger<SendEmbedToUserHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Unit> Handle(SendEmbedToUserCommand request, CancellationToken ct)
        {
            var user = await _mediator.Send(new GetUserQuery((long) request.UserId));
            var socketUser = await _mediator.Send(new GetSocketGuildUserQuery(request.GuildId, request.UserId));

            try
            {
                await socketUser.SendMessageAsync(
                    text: request.Text,
                    embed: request.EmbedBuilder.WithUserColor(user.CommandColor).Build(),
                    components: request.Components);

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