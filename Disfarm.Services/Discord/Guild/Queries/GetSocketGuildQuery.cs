using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Disfarm.Services.Discord.Client;
using MediatR;

namespace Disfarm.Services.Discord.Guild.Queries
{
    public record GetSocketGuildQuery(ulong GuildId) : IRequest<SocketGuild>;

    public class GetSocketGuildHandler : IRequestHandler<GetSocketGuildQuery, SocketGuild>
    {
        private readonly IDiscordClientService _discordClientService;

        public GetSocketGuildHandler(IDiscordClientService discordClientService)
        {
            _discordClientService = discordClientService;
        }

        public async Task<SocketGuild> Handle(GetSocketGuildQuery request, CancellationToken ct)
        {
            var client = await _discordClientService.GetSocketClient();
            var guild = client.GetGuild(request.GuildId);

            if (guild is null)
            {
                throw new Exception(
                    $"socket guild {request.GuildId} was not found.");
            }

            return guild;
        }
    }
}