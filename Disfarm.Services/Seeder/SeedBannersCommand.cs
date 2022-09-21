using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Guild.Queries;
using Disfarm.Services.Game.Banner.Commands;
using MediatR;
using Microsoft.Extensions.Options;

namespace Disfarm.Services.Seeder
{
    public record SeedBannersCommand : IRequest<TotalAndAffectedCountDto>;

    public class SeedBannersHandler : IRequestHandler<SeedBannersCommand, TotalAndAffectedCountDto>
    {
        private readonly IMediator _mediator;
        private readonly DiscordClientOptions _options;

        public SeedBannersHandler(
            IMediator mediator,
            IOptions<DiscordClientOptions> options)
        {
            _mediator = mediator;
            _options = options.Value;
        }

        public async Task<TotalAndAffectedCountDto> Handle(SeedBannersCommand request, CancellationToken ct)
        {
            var result = new TotalAndAffectedCountDto();
            var commands = new List<CreateBannerCommand>();

            var guild = await _mediator.Send(new GetSocketGuildQuery(_options.FilesGuildId));
            var bannerRarities = Enum
                .GetValues(typeof(BannerRarity))
                .Cast<BannerRarity>();

            foreach (var rarity in bannerRarities)
            {
                var channel = guild.TextChannels.First(x => x.Name == "banner-" + rarity.ToString().ToLower());
                var messages = await channel.GetMessagesAsync().FlattenAsync();

                commands.AddRange(messages.Select(message => new CreateBannerCommand(
                    Name: message.Attachments.First().Filename[..message.Attachments.First().Filename.LastIndexOf('.')],
                    Rarity: rarity,
                    Price: rarity switch
                    {
                        BannerRarity.Common => 3150,
                        BannerRarity.Rare => 4800,
                        BannerRarity.Animated => 7200,
                        BannerRarity.Limited => 9999,
                        BannerRarity.Custom => 9999,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    Url: message.Attachments.First().Url)));
            }

            foreach (var createBannerCommand in commands)
            {
                result.Total++;

                try
                {
                    await _mediator.Send(createBannerCommand);

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