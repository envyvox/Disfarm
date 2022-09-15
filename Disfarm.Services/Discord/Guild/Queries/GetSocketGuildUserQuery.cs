#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Discord.Guild.Queries
{
    public record GetSocketGuildUserQuery(ulong GuildId, ulong UserId) : IRequest<SocketGuildUser>;

    public class GetSocketGuildUserHandler : IRequestHandler<GetSocketGuildUserQuery, SocketGuildUser>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GetSocketGuildUserHandler> _logger;

        public GetSocketGuildUserHandler(
            IMediator mediator,
            ILogger<GetSocketGuildUserHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<SocketGuildUser> Handle(GetSocketGuildUserQuery request, CancellationToken ct)
        {
            var socketGuild = await _mediator.Send(new GetSocketGuildQuery(request.GuildId));

            await socketGuild.DownloadUsersAsync();

            var socketUser = socketGuild.GetUser(request.UserId);

            if (socketUser is null)
            {
                throw new Exception(
                    $"socket user {request.UserId} not found in guild {request.GuildId}");
            }

            return socketUser;
        }
    }
}