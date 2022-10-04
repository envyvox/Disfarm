using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Guild.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Currency.Commands;
using Disfarm.Services.Game.Currency.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Disfarm.Services.Extensions.ExceptionExtensions;

namespace Disfarm.Services.Discord.Interactions.Commands
{
    [RequireContext(ContextType.Guild)]
    public class SendCurrency : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public SendCurrency(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [SlashCommand("send-currency", "Send currency to specified user")]
        public async Task Execute(
            [Summary("user", "The user to whom you want to transfer the currency (@mention or ID)")]
            IUser mentionedUser,
            [Summary("currency", "The type of currency you would like to transfer")]
            Currency currency,
            [Summary("amount", "The amount you would like to transfer")] [MinValue(1)]
            uint amount)
        {
            await DeferAsync();

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, currency));

            if (userCurrency.Amount < amount)
            {
                throw new GameUserExpectedException(Response.SendCurrencyNoCurrency.Parse(user.Language,
                    emotes.GetEmote(currency.ToString()),
                    _local.Localize(LocalizationCategory.Currency, currency.ToString(), user.Language, 5)));
            }

            var targetUser = await _mediator.Send(new GetUserQuery((long) mentionedUser.Id));
            var socketTargetUser = await _mediator.Send(new GetSocketGuildUserQuery(
                Context.Guild.Id, mentionedUser.Id));
            var mention = socketTargetUser is null
                ? $"{emotes.GetEmote(targetUser.Title.EmoteName())} {targetUser.Title.Localize(user.Language)} <@{mentionedUser.Id}>"
                : socketTargetUser.Mention.AsGameMention(targetUser.Title, user.Language);

            await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, currency, amount));
            await _mediator.Send(new AddCurrencyToUserCommand(targetUser.Id, currency, amount));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.SendCurrencyAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithDescription(Response.SendCurrencyDesc.Parse(user.Language,
                    Context.User.Mention.AsGameMention(user.Title, user.Language),
                    emotes.GetEmote(currency.ToString()), amount,
                    _local.Localize(LocalizationCategory.Currency, currency.ToString(), user.Language, amount),
                    mention));

            await Context.Interaction.FollowUpResponse(embed);

            var notifyEmbed = new EmbedBuilder()
                .WithUserColor(targetUser.CommandColor)
                .WithAuthor(Response.SendCurrencyAuthor.Parse(targetUser.Language), socketTargetUser?.GetAvatarUrl())
                .WithDescription(Response.SendCurrencyNotifyDesc.Parse(targetUser.Language,
                    mention, Context.User.Mention.AsGameMention(user.Title, targetUser.Language),
                    emotes.GetEmote(currency.ToString()), amount,
                    _local.Localize(LocalizationCategory.Currency, currency.ToString(), targetUser.Language, amount)));

            await _mediator.Send(new SendEmbedToUserCommand(
                Context.Guild.Id, Context.Channel.Id, mentionedUser.Id, notifyEmbed, ShowLinkButton: false));
        }
    }
}