using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.DailyReward.Queries;
using Disfarm.Services.Game.User.Queries;
using MediatR;

namespace Disfarm.Services.Discord.Interactions.Commands
{
    [RequireContext(ContextType.Guild)]
    public class DailyReward : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly TimeZoneInfo _timeZoneInfo;

        public DailyReward(
            IMediator mediator,
            TimeZoneInfo timeZoneInfo)
        {
            _mediator = mediator;
            _timeZoneInfo = timeZoneInfo;
        }

        [SlashCommand("daily-reward", "Get rewarded for daily activity in the game world")]
        public async Task Execute()
        {
            await DeferAsync(true);

            var timeNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _timeZoneInfo);
            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var hasTodayReward = await _mediator.Send(new CheckUserDailyRewardQuery(
                user.Id, timeNow.DayOfWeek));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.DailyRewardAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(Response.DailyRewardDesc.Parse(user.Language,
                    Context.User.Mention.AsGameMention(user.Title, user.Language), emotes.GetEmote("Arrow")))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.DailyReward, user.Language)));

            var components = new ComponentBuilder()
                .WithButton(
                    Response.ComponentReceiveDailyReward.Parse(user.Language),
                    "receive-daily-reward",
                    ButtonStyle.Success,
                    disabled: hasTodayReward)
                .WithButton(
                    Response.ComponentShowDailyRewards.Parse(user.Language),
                    "show-daily-rewards",
                    ButtonStyle.Secondary)
                .Build();

            await Context.Interaction.FollowUpResponse(embed, components);
        }
    }
}