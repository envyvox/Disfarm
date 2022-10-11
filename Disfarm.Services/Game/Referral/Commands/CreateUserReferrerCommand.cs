using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Disfarm.Data;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Disfarm.Services.Discord.Client.Queries;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Guild.Queries;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Banner.Commands;
using Disfarm.Services.Game.Banner.Queries;
using Disfarm.Services.Game.Container.Commands;
using Disfarm.Services.Game.Currency.Commands;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Referral.Queries;
using Disfarm.Services.Game.Title.Commands;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Image = Disfarm.Data.Enums.Image;

namespace Disfarm.Services.Game.Referral.Commands
{
    public record CreateUserReferrerCommand(
            ulong GuildId,
            ulong ChannelId,
            long UserId,
            long ReferrerId)
        : IRequest;

    public class CreateUserReferrerHandler : IRequestHandler<CreateUserReferrerCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private readonly ILogger<CreateUserReferrerHandler> _logger;
        private readonly AppDbContext _db;

        public CreateUserReferrerHandler(
            DbContextOptions options,
            IMediator mediator,
            ILocalizationService local,
            ILogger<CreateUserReferrerHandler> logger)
        {
            _mediator = mediator;
            _local = local;
            _logger = logger;
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateUserReferrerCommand request, CancellationToken ct)
        {
            var exist = await EntityFrameworkQueryableExtensions.AnyAsync(_db.UserReferrers,
                x => x.UserId == request.UserId);

            if (exist)
            {
                throw new Exception(
                    $"user {request.UserId} already have referrer");
            }

            var created = await _db.CreateEntity(new UserReferrer
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ReferrerId = request.ReferrerId,
                CreatedAt = DateTimeOffset.UtcNow
            });

            _logger.LogInformation(
                "Created user referrer entity {@Entity}",
                created);

            await AddRewardsToReferrer(request.GuildId, request.ChannelId, request.UserId, request.ReferrerId);

            return Unit.Value;
        }

        private async Task AddRewardsToReferrer(ulong guildId, ulong channelId, long userId, long referrerId)
        {
            var emotes = DiscordRepository.Emotes;
            var referralCount = await _mediator.Send(new GetUserReferralCountQuery(referrerId));
            var user = await _mediator.Send(new GetUserQuery(userId));
            var referrer = await _mediator.Send(new GetUserQuery(referrerId));

            var rewardString = string.Empty;
            switch (referralCount)
            {
                case 1 or 2:

                    await _mediator.Send(new AddContainerToUserCommand(referrerId, Data.Enums.Container.Token, 1));

                    rewardString =
                        $"{emotes.GetEmote(Data.Enums.Container.Token.EmoteName())} {_local.Localize(LocalizationCategory.Container, Data.Enums.Container.Token.ToString(), referrer.Language)}";

                    break;

                case 3 or 4:

                    await _mediator.Send(new AddContainerToUserCommand(referrerId, Data.Enums.Container.Token, 2));

                    rewardString =
                        $"{emotes.GetEmote(Data.Enums.Container.Token.EmoteName())} 2 " +
                        $"{_local.Localize(LocalizationCategory.Container, Data.Enums.Container.Token.ToString(), referrer.Language, 2)}";

                    break;

                case 5:

                    var banners = await _mediator.Send(new GetBannersQuery());
                    var banner = banners.Single(x => x.Name == "BibaAndBoba");

                    await _mediator.Send(new AddContainerToUserCommand(referrerId, Data.Enums.Container.Token, 5));
                    await _mediator.Send(new AddBannerToUserCommand(referrerId, banner.Id, null));

                    rewardString = Response.ReferrerRewardsBanner.Parse(referrer.Language,
                        emotes.GetEmote(Data.Enums.Container.Token.EmoteName()),
                        _local.Localize(LocalizationCategory.Container, Data.Enums.Container.Token.ToString(),
                            referrer.Language, 5),
                        emotes.GetEmote(banner.Rarity.EmoteName()), banner.Rarity.Localize(referrer.Language).ToLower(),
                        banner.Name);

                    break;

                case 6 or 7 or 8 or 9:

                    await _mediator.Send(new AddCurrencyToUserCommand(referrerId, Data.Enums.Currency.Chip, 10));

                    rewardString =
                        $"{emotes.GetEmote(Data.Enums.Currency.Chip.ToString())} 10 " +
                        $"{_local.Localize(LocalizationCategory.Currency, Data.Enums.Currency.Chip.ToString(), referrer.Language, 10)}";

                    break;

                case 10:

                    await _mediator.Send(new AddCurrencyToUserCommand(referrerId, Data.Enums.Currency.Chip, 10));
                    await _mediator.Send(new AddTitleToUserCommand(referrerId, Data.Enums.Title.Yatagarasu));

                    rewardString = Response.ReferrerRewardsTitle.Parse(referrer.Language,
                        emotes.GetEmote(Data.Enums.Currency.Chip.ToString()),
                        _local.Localize(LocalizationCategory.Currency, Data.Enums.Currency.Chip.ToString(),
                            referrer.Language, 10),
                        emotes.GetEmote(Data.Enums.Title.Yatagarasu.EmoteName()),
                        Data.Enums.Title.Yatagarasu.Localize(referrer.Language));

                    break;

                case > 10:

                    await _mediator.Send(new AddCurrencyToUserCommand(referrerId, Data.Enums.Currency.Chip, 15));

                    rewardString =
                        $"{emotes.GetEmote(Data.Enums.Currency.Chip.ToString())} 15 " +
                        $"{_local.Localize(LocalizationCategory.Currency, Data.Enums.Currency.Chip.ToString(), referrer.Language, 15)}";

                    break;
            }

            var socketUser = await _mediator.Send(new GetClientUserQuery((ulong) user.Id));
            var socketReferrer = await _mediator.Send(new GetClientUserQuery((ulong) referrer.Id));

            var embed = new EmbedBuilder()
                .WithAuthor(Response.ReferralSystemAuthor.Parse(referrer.Language), socketReferrer?.GetAvatarUrl())
                .WithDescription(Response.ReferrerRewardsDesc.Parse(referrer.Language,
                    socketReferrer?.Mention.AsGameMention(referrer.Title, referrer.Language),
                    socketUser?.Mention.AsGameMention(user.Title, referrer.Language),
                    rewardString, emotes.GetEmote("Arrow")))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Referral, user.Language)));

            await _mediator.Send(new SendEmbedToUserCommand(guildId, channelId, (ulong) referrerId, embed,
                ShowLinkButton: false));
        }
    }
}