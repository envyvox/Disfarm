using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Guild.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Banner.Commands;
using Disfarm.Services.Game.Banner.Queries;
using Disfarm.Services.Game.Calculation;
using Disfarm.Services.Game.Container.Commands;
using Disfarm.Services.Game.Currency.Commands;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Title.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.User.Commands
{
    public record AddXpToUserCommand(ulong GuildId, long UserId, uint Amount) : IRequest;

    public class AddXpToUserHandler : IRequestHandler<AddXpToUserCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AddXpToUserHandler> _logger;
        private readonly ILocalizationService _local;
        private readonly AppDbContext _db;

        public AddXpToUserHandler(
            DbContextOptions options,
            IMediator mediator,
            ILogger<AddXpToUserHandler> logger,
            ILocalizationService local)
        {
            _db = new AppDbContext(options);
            _mediator = mediator;
            _logger = logger;
            _local = local;
        }

        public async Task<Unit> Handle(AddXpToUserCommand request, CancellationToken ct)
        {
            var entity = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync(_db.Users,
                x => x.Id == request.UserId);

            entity.Xp += request.Amount;
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await _db.UpdateEntity(entity);

            _logger.LogInformation(
                "Added xp to user {UserId} amount {Amount}",
                request.UserId, request.Amount);

            await CheckUserLevelUp(entity, request.GuildId);

            return Unit.Value;
        }

        private async Task CheckUserLevelUp(Data.Entities.User.User user, ulong guildId)
        {
            var xpRequired = await _mediator.Send(new GetRequiredXpQuery(user.Level + 1));

            if (user.Xp > xpRequired)
            {
                user.Level++;
                user.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(user);

                _logger.LogInformation(
                    "Updated user {UserId} level to {Level}",
                    user.Id, user.Level);

                await AddLevelUpReward(user, guildId);
            }
        }

        private async Task AddLevelUpReward(Data.Entities.User.User user, ulong guildId)
        {
            var emotes = DiscordRepository.Emotes;
            var socketUser = await _mediator.Send(new GetSocketGuildUserQuery(guildId, (ulong) user.Id));

            string rewardString;
            switch (user.Level)
            {
                // titles
                case 5 or 30 or 100:
                {
                    var title = user.Level switch
                    {
                        5 => Data.Enums.Title.FirstSamurai,
                        30 => Data.Enums.Title.Newbie, // todo change title
                        100 => Data.Enums.Title.Newbie, // todo change title
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    await _mediator.Send(new AddTitleToUserCommand(user.Id, title));

                    rewardString = Response.LevelUpRewardTitle.Parse(user.Language,
                        emotes.GetEmote(title.EmoteName()), title.Localize(user.Language), emotes.GetEmote("Arrow"));

                    break;
                }
                // chips
                case 15 or 20 or 25 or 35 or 40 or 45 or 55 or 60 or 65 or 70 or 75 or 85 or 90 or 95:
                {
                    var amount = user.Level / 10 * 5;

                    await _mediator.Send(new AddCurrencyToUserCommand(user.Id, Data.Enums.Currency.Chip, amount));

                    rewardString = Response.LevelUpRewardChips.Parse(user.Language,
                        emotes.GetEmote(Data.Enums.Currency.Chip.ToString()), amount,
                        _local.Localize(LocalizationCategory.Currency, Data.Enums.Currency.Chip.ToString(),
                            user.Language, amount), emotes.GetEmote("Arrow"));

                    break;
                }
                // banners
                case 10 or 50 or 80:
                {
                    var banners = await _mediator.Send(new GetBannersQuery());
                    var banner = banners.Single(x => x.Name == user.Level switch
                    {
                        10 => "FirstSamurai",
                        50 => "", // todo add banner name
                        80 => "", // todo add banner name
                        _ => throw new ArgumentOutOfRangeException()
                    });

                    await _mediator.Send(new AddBannerToUserCommand(user.Id, banner.Id, null));

                    rewardString = Response.LevelUpRewardBanner.Parse(user.Language,
                        emotes.GetEmote(banner.Rarity.EmoteName()), banner.Rarity.Localize(user.Language).ToLower(),
                        _local.Localize(LocalizationCategory.Banner, banner.Name, user.Language),
                        emotes.GetEmote("Arrow"));

                    break;
                }
                // containers
                default:
                {
                    var amount = user.Level / 10 + 1;

                    await _mediator.Send(new AddContainerToUserCommand(user.Id, Data.Enums.Container.Token, amount));

                    rewardString = Response.LevelUpRewardContainer.Parse(user.Language,
                        emotes.GetEmote(Data.Enums.Container.Token.EmoteName()), amount,
                        _local.Localize(LocalizationCategory.Container, Data.Enums.Container.Token.ToString(),
                            user.Language, amount), emotes.GetEmote("Arrow"));

                    break;
                }
            }

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.LevelUpRewardAuthor.Parse(user.Language), socketUser.GetAvatarUrl())
                .WithDescription(Response.LevelUpRewardDesc.Parse(user.Language,
                    socketUser.Mention.AsGameMention(user.Title, user.Language), emotes.GetEmote("Xp"),
                    user.Level.AsLevelEmote(), user.Level, rewardString));

            await _mediator.Send(new SendEmbedToUserCommand(guildId, socketUser.Id, embed));
        }
    }
}