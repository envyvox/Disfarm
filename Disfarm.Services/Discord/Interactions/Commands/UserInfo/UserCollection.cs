using System;
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
using Disfarm.Services.Game.Collection.Queries;
using Disfarm.Services.Game.Crop.Queries;
using Disfarm.Services.Game.Fish.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using MediatR;

namespace Disfarm.Services.Discord.Interactions.Commands.UserInfo
{
	[RequireGuildContext]
	public class UserCollection : InteractionModuleBase<SocketInteractionContext>
	{
		private readonly IMediator _mediator;
		private readonly ILocalizationService _local;

		public UserCollection(
			IMediator mediator,
			ILocalizationService local)
		{
			_mediator = mediator;
			_local = local;
		}

		[SlashCommand("collection", "View your collection")]
		public async Task Execute(
			[Summary("category", "The category of the collection you want to see")]
			CollectionCategory category)
		{
			await DeferAsync(true);

			var emotes = DiscordRepository.Emotes;
			var user = await _mediator.Send(new GetUserQuery((long)Context.User.Id));
			var userCollections = await _mediator.Send(new GetUserCollectionsQuery(user.Id, category));

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.UserCollectionAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
				.WithDescription(
					Response.UserCollectionDesc.Parse(user.Language,
						Context.User.Mention.AsGameMention(user.Title, user.Language),
						category.Localize(user.Language)) +
					$"\n{StringExtensions.EmptyChar}")
				.WithImageUrl(
					await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.UserCollection, user.Language)));

			switch (category)
			{
				case CollectionCategory.Crop:

					var crops = await _mediator.Send(new GetCropsQuery());
					var springCropString = string.Empty;
					var summerCropString = string.Empty;
					var autumnCropString = string.Empty;

					foreach (var crop in crops)
					{
						var exist = userCollections.Any(x => x.ItemId == crop.Id);
						var displayString =
							$"{emotes.GetEmote(crop.Name + (exist ? "" : "BW"))} " +
							$"{_local.Localize(LocalizationCategory.Crop, crop.Name, user.Language)} ";

						switch (crop.Seed.Season)
						{
							case Season.Spring:
								springCropString += displayString;
								break;
							case Season.Summer:
								summerCropString += displayString;
								break;
							case Season.Autumn:
								autumnCropString += displayString;
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}
					}

					embed
						.AddField(Response.UserCollectionCropString.Parse(user.Language), springCropString)
						.AddField(Response.UserCollectionCropSummer.Parse(user.Language), summerCropString)
						.AddField(Response.UserCollectionCropAutumn.Parse(user.Language), autumnCropString);

					break;
				case CollectionCategory.Fish:

					var fishes = await _mediator.Send(new GetFishesQuery());
					var commonFishString = string.Empty;
					var rareFishString = string.Empty;
					var epicFishString = string.Empty;
					var mythicalFishString = string.Empty;
					var legendaryFishString = string.Empty;

					foreach (var fish in fishes)
					{
						var exist = userCollections.Any(x => x.ItemId == fish.Id);
						var displayString =
							$"{emotes.GetEmote(fish.Name + (exist ? "" : "BW"))} " +
							$"{_local.Localize(LocalizationCategory.Fish, fish.Name, user.Language)} ";

						switch (fish.Rarity)
						{
							case FishRarity.Common:
								commonFishString += displayString;
								break;
							case FishRarity.Rare:
								rareFishString += displayString;
								break;
							case FishRarity.Epic:
								epicFishString += displayString;
								break;
							case FishRarity.Mythical:
								mythicalFishString += displayString;
								break;
							case FishRarity.Legendary:
								legendaryFishString += displayString;
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}
					}

					embed
						.AddField(FishRarity.Common.Localize(user.Language), commonFishString)
						.AddField(FishRarity.Rare.Localize(user.Language), rareFishString)
						.AddField(FishRarity.Epic.Localize(user.Language), epicFishString)
						.AddField(FishRarity.Mythical.Localize(user.Language), mythicalFishString)
						.AddField(FishRarity.Legendary.Localize(user.Language), legendaryFishString);

					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			await Context.Interaction.FollowUpResponse(embed);
		}
	}
}