using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Container.Commands;
using Disfarm.Services.Game.Container.Queries;
using Disfarm.Services.Game.Currency.Commands;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using static Disfarm.Services.Extensions.ExceptionExtensions;

namespace Disfarm.Services.Discord.Interactions.Components
{
    public class ContainerOpen : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private readonly Random _random = new();

        public ContainerOpen(
            IMediator mediator,
            ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [ComponentInteraction("container-open:*")]
        public async Task Execute(string containerHashcodeString)
        {
            await DeferAsync();

            var container = (Container) int.Parse(containerHashcodeString);

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
            var userContainer = await _mediator.Send(new GetUserContainerQuery(user.Id, container));

            if (userContainer.Amount < 1)
            {
                throw new GameUserExpectedException(Response.ContainerOpenNoContainers.Parse(user.Language,
                    emotes.GetEmote(container.EmoteName()),
                    _local.Localize(LocalizationCategory.Container, container.ToString(), user.Language, 2)));
            }

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.ContainerOpenAuthor.Parse(user.Language), Context.User.GetAvatarUrl());

            switch (container)
            {
                case Container.Token:
                {
                    var minAmount = (int) await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.ContainerTokenMinAmount));
                    var maxAmount = (int) await _mediator.Send(new GetWorldPropertyValueQuery(
                        WorldProperty.ContainerTokenMaxAmount));

                    var receivedAmount = 0;
                    for (var i = 0; i < userContainer.Amount; i++)
                    {
                        receivedAmount += _random.Next(minAmount, maxAmount + 1);
                    }

                    await _mediator.Send(new RemoveContainerFromUserCommand(user.Id, container, userContainer.Amount));
                    await _mediator.Send(new AddCurrencyToUserCommand(user.Id, Currency.Token, (uint) receivedAmount));

                    embed.WithDescription(Response.ContainerOpenTokenDesc.Parse(user.Language,
                        Context.User.Mention.AsGameMention(user.Title, user.Language),
                        emotes.GetEmote(container.EmoteName()), userContainer.Amount,
                        _local.Localize(LocalizationCategory.Container, container.ToString(), user.Language,
                            userContainer.Amount), emotes.GetEmote(Currency.Token.ToString()), receivedAmount,
                        _local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
                            (uint) receivedAmount)));

                    break;
                }

                case Container.Supply:
                {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await Context.Interaction.FollowUpResponse(embed);
        }
    }
}