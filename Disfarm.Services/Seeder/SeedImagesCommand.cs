using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Disfarm.Services.Discord.Client;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Commands;
using MediatR;
using Microsoft.Extensions.Options;
using Image = Disfarm.Data.Enums.Image;

namespace Disfarm.Services.Seeder
{
    public record SeedImagesCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeedImagesHandler : IRequestHandler<SeedImagesCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;
        private readonly IDiscordClientService _discordClientService;
        private readonly DiscordClientOptions _options;

        public SeedImagesHandler(
            IMediator mediator,
            IDiscordClientService discordClientService,
            IOptions<DiscordClientOptions> options)
        {
            _mediator = mediator;
            _discordClientService = discordClientService;
            _options = options.Value;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeedImagesCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var socketClient = await _discordClientService.GetSocketClient();
            var guild = socketClient.GetGuild(_options.FilesGuildId);

            var languages = Enum
                .GetValues(typeof(Language))
                .Cast<Language>();
            var imageTypes = Enum
                .GetValues(typeof(Image))
                .Cast<Image>();

            var commands = new List<CreateImageCommand>();

            foreach (var language in languages)
            {
                var channel = guild.TextChannels.First(x => x.Name == "images-" + language.ToString().ToLower());
                var messages = await channel.GetMessagesAsync().FlattenAsync();

                commands.AddRange(from message in messages
                    from attachment in message.Attachments
                    from imageType in imageTypes
                    where attachment.Filename[..attachment.Filename.LastIndexOf('.')] == imageType.ToString()
                    select new CreateImageCommand(imageType, language, attachment.Url));
            }

            foreach (var createImageCommand in commands)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(createImageCommand);

                    result.Affected++;
                }
                catch
                {
                    // ignored
                }
            }

            return result;
        }
    }
}