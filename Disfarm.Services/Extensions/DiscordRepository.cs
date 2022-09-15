using System.Collections.Generic;
using Disfarm.Services.Discord.Emote.Models;

namespace Disfarm.Services.Extensions
{
    public static class DiscordRepository
    {
        public static readonly Dictionary<string, EmoteDto> Emotes = new();
    }
}

