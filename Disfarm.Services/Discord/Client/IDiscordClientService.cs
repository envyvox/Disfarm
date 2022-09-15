using System.Threading.Tasks;
using Discord.WebSocket;

namespace Disfarm.Services.Discord.Client
{
    public interface IDiscordClientService
    {
        public Task Start();

        public Task<DiscordSocketClient> GetSocketClient();
    }
}