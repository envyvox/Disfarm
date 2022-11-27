using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Farm.Commands;
using Disfarm.Services.Game.Farm.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Discord.Emote;

namespace Disfarm.Services.Discord.Interactions.Components.Farm
{
	[RequireLocation(Location.Neutral)]
	public class UserFarmDig : InteractionModuleBase<SocketInteractionContext>
	{
		private readonly IMediator _mediator;
		private readonly ILocalizationService _local;

		public UserFarmDig(
			IMediator mediator,
			ILocalizationService local)
		{
			_mediator = mediator;
			_local = local;
		}

		[ComponentInteraction("ufd")]
		public async Task Execute()
		{
			await DeferAsync();

			var emotes = DiscordRepository.Emotes;
			var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
			var userFarms = await _mediator.Send(new GetUserFarmsQuery(user.Id));

			userFarms = userFarms
				.Where(x => x.State is not FieldState.Empty)
				.ToList();

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.UserFarmDigAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
				.WithDescription(Response.UserFarmDigDesc.Parse(user.Language,
					Context.User.Mention.AsGameMention(user.Title, user.Language),
					emotes.GetEmote(Building.Farm.ToString()), emotes.GetEmote("Arrow")))
				.WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Harvesting, user.Language)));

			var selectMenu = new SelectMenuBuilder()
				.WithPlaceholder(Response.ComponentUserFarmDigSelect.Parse(user.Language))
				.WithCustomId("fds")
				.WithMaxValues(userFarms.Count);

			foreach (var userFarm in userFarms)
			{
				selectMenu.AddOption(
					Response.ComponentUserFarmDigSelectLabel.Parse(user.Language, userFarm.Number),
					$"{userFarm.Number}",
					Response.ComponentUserFarmDigSelectDesc.Parse(user.Language,
						_local.Localize(LocalizationCategory.Seed, userFarm.Seed.Name, user.Language), userFarm.Number),
					Parse(emotes.GetEmote(userFarm.Seed.Name)));
			}

			var components = new ComponentBuilder()
				.WithSelectMenu(selectMenu)
				.WithButton(
					Response.ComponentUserFarmDigSelectAll.Parse(user.Language),
					$"das:{string.Join('.', userFarms.Select(x => x.Number))}",
					ButtonStyle.Danger);

			await ModifyOriginalResponseAsync(x =>
			{
				x.Embed = embed.Build();
				x.Components = components.Build();
			});
		}

		[ComponentInteraction("fds")]
		public async Task Execute(string[] selectedValues)
		{
			await DeferAsync();

			var emotes = DiscordRepository.Emotes;
			var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
			var userFarms = await _mediator.Send(new GetUserFarmsQuery(user.Id));

			userFarms = userFarms
				.Where(x => selectedValues.Select(uint.Parse).Contains(x.Number))
				.ToList();

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.UserFarmDigAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
				.WithDescription(Response.UserFarmDigCompleted.Parse(user.Language,
					Context.User.Mention.AsGameMention(user.Title, user.Language),
					emotes.GetEmote(Building.Farm.ToString())))
				.WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Harvesting, user.Language)));

			foreach (var userFarm in userFarms)
			{
				await _mediator.Send(new ResetUserFarmCommand(user.Id, userFarm.Number));
			}

			await Context.Interaction.FollowUpResponse(embed);
			await Context.Interaction.ClearOriginalResponse(user.Language);
		}

		[ComponentInteraction("das:*")]
		public async Task Execute(string selectedCellsString) =>
			await Execute(selectedCellsString.Split('.'));
	}
}