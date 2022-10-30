using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Building.Commands;
using Disfarm.Services.Game.Currency.Commands;
using Disfarm.Services.Game.Currency.Queries;
using Disfarm.Services.Game.Farm.Commands;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using static Disfarm.Services.Extensions.ExceptionExtensions;

namespace Disfarm.Services.Discord.Interactions.Components.Farm
{
	public class UserFarmBuy : InteractionModuleBase<SocketInteractionContext>
	{
		private readonly IMediator _mediator;
		private readonly ILocalizationService _local;

		public UserFarmBuy(
			IMediator mediator,
			ILocalizationService local)
		{
			_mediator = mediator;
			_local = local;
		}

		[ComponentInteraction("farm-buy")]
		public async Task Execute()
		{
			await DeferAsync();

			var emotes = DiscordRepository.Emotes;
			var user = await _mediator.Send(new GetUserQuery((long)Context.User.Id));
			var userCurrency = await _mediator.Send(new GetUserCurrencyQuery(user.Id, Currency.Token));
			var farmPrice = await _mediator.Send(new GetWorldPropertyValueQuery(WorldProperty.FarmPrice));

			if (userCurrency.Amount < farmPrice)
			{
				throw new GameUserExpectedException(Response.UserFarmBuyNoCurrency.Parse(user.Language,
					emotes.GetEmote(Currency.Token.ToString()),
					_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language, 5),
					emotes.GetEmote(Building.Farm.ToString())));
			}

			await _mediator.Send(new RemoveCurrencyFromUserCommand(user.Id, Currency.Token, farmPrice));
			await _mediator.Send(new CreateUserBuildingCommand(user.Id, Building.Farm));
			await _mediator.Send(new CreateUserFarmsCommand(user.Id, new uint[] { 1, 2, 3, 4, 5 }));

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.UserFarmBuyAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
				.WithDescription(Response.UserFarmBuyDesc.Parse(user.Language,
					Context.User.Mention.AsGameMention(user.Title, user.Language),
					emotes.GetEmote(Building.Farm.ToString()), emotes.GetEmote(Currency.Token.ToString()), farmPrice,
					_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
						farmPrice)))
				.WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Harvesting, user.Language)));

			await Context.Interaction.FollowUpResponse(embed);
			await Context.Interaction.ClearOriginalResponse(user.Language);
		}
	}
}