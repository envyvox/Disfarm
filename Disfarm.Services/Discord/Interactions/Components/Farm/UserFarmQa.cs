using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Building.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Components.Farm
{
    [RequireLocation(Location.Neutral)]
    public class UserFarmQa : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public UserFarmQa(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [ComponentInteraction("user-farm-qa:*")]
        public async Task Execute(string selectedQuestion)
        {
            await DeferAsync(true);

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Farm, user.Language)));

            var components = new ComponentBuilder();

            switch (selectedQuestion)
            {
                case "harvesting":
                {
                    embed
                        .WithAuthor(Response.UserFarmQaHarvestingAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                        .WithDescription(Response.UserFarmQaHarvestingDesc.Parse(user.Language,
                            Context.User.Mention.AsGameMention(user.Title, user.Language),
                            emotes.GetEmote("Arrow"), emotes.GetEmote("DiscordSlashCommand"),
                            emotes.GetEmote(Building.Farm.ToString())));

                    break;
                }
                case "upgrading":
                {
                    var hasFarmExpansionL1 = await _mediator.Send(new CheckUserHasBuildingQuery(
                        user.Id, Building.FarmExpansionL1));
                    var hasFarmExpansionL2 = await _mediator.Send(new CheckUserHasBuildingQuery(
                        user.Id, Building.FarmExpansionL2));
                    var expansionL1Price = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.FarmExpansionL1Price));
                    var expansionL2Price = await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.FarmExpansionL2Price));

                    embed
                        .WithAuthor(Response.UserFarmQaUpgradingAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                        .WithDescription(Response.UserFarmQaUpgradingDesc.Parse(user.Language,
                            Context.User.Mention.AsGameMention(user.Title, user.Language),
                            emotes.GetEmote(Building.Farm.ToString()), emotes.GetEmote("Arrow"),
                            emotes.GetEmote(Building.FarmExpansionL1.ToString()),
                            emotes.GetEmote(Currency.Token.ToString()), expansionL1Price,
                            _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                                expansionL1Price),
                            emotes.GetEmote(Building.FarmExpansionL2.ToString()), expansionL2Price,
                            _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                                expansionL2Price)));

                    components.WithButton(
                        Response.ComponentFarmUpgrade.Parse(user.Language),
                        @$"user-farm-upgrade:{(hasFarmExpansionL1
                            ? Building.FarmExpansionL2.GetHashCode()
                            : Building.FarmExpansionL1.GetHashCode())}",
                        emote: Parse(emotes.GetEmote(hasFarmExpansionL1
                            ? Building.FarmExpansionL2.ToString()
                            : Building.FarmExpansionL1.ToString())),
                        disabled: hasFarmExpansionL2);

                    break;
                }
            }

            await ModifyOriginalResponseAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = components.Build();
            });
        }
    }
}