using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Data.Enums.Achievement;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Achievement.Commands;
using Disfarm.Services.Game.Farm.Commands;
using Disfarm.Services.Game.Farm.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Seed.Commands;
using Disfarm.Services.Game.Seed.Queries;
using Disfarm.Services.Game.Statistic.Commands;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using static Discord.Emote;
using static Disfarm.Services.Extensions.ExceptionExtensions;

namespace Disfarm.Services.Discord.Interactions.Components.Farm
{
	[RequireLocation(Location.Neutral)]
	public class FarmPlant : InteractionModuleBase<SocketInteractionContext>
	{
		private readonly IMediator _mediator;
		private readonly ILocalizationService _local;
		private readonly TimeZoneInfo _timeZoneInfo;

		public FarmPlant(
			IMediator mediator,
			ILocalizationService local,
			TimeZoneInfo timeZoneInfo)
		{
			_mediator = mediator;
			_local = local;
			_timeZoneInfo = timeZoneInfo;
		}

		/// <summary>
		/// Step 1 - Select seeds
		/// </summary>
		[ComponentInteraction("ufp:*")]
		public async Task Execute(string pageString)
		{
			await DeferAsync(true);

			var timeNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _timeZoneInfo);
			var page = int.Parse(pageString);
			var emotes = DiscordRepository.Emotes;
			var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
			var userSeeds = await _mediator.Send(new GetUserSeedsQuery(user.Id));

			if (userSeeds.Count < 1)
			{
				throw new GameUserExpectedException(Response.UserFarmPlantNoSeeds.Parse(user.Language,
					emotes.GetEmote(Building.Farm.ToString()), emotes.GetEmote("Arrow")));
			}

			var maxPage = (int) Math.Ceiling(userSeeds.Count / 5.0);
			maxPage = maxPage > 0 ? maxPage : 1; // just for better display

			userSeeds = userSeeds
				.Skip(page > 1 ? (page - 1) * 5 : 0)
				.Take(5)
				.ToList();

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.UserFarmPlantAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
				.WithDescription(
					Response.UserFarmPlantSelectSeedsDesc.Parse(user.Language,
						Context.User.Mention.AsGameMention(user.Title, user.Language),
						emotes.GetEmote(Building.Farm.ToString()),
						emotes.GetEmote("Arrow")) +
					$"\n{StringExtensions.EmptyChar}")
				.WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Harvesting, user.Language)))
				.WithFooter(Response.PaginatorFooter.Parse(user.Language, page, maxPage));

			var selectMenu = new SelectMenuBuilder()
				.WithPlaceholder(Response.ComponentUserFarmPlantSelectSeed.Parse(user.Language))
				.WithCustomId("pss");

			foreach (var userSeed in userSeeds)
			{
				var seedDesc = Response.ShopSeedSeedDesc.Parse(user.Language,
					timeNow.Add(userSeed.Seed.Growth).ToDiscordTimestamp(TimestampFormat.RelativeTime),
					emotes.GetEmote(userSeed.Seed.Crop.Name),
					_local.Localize(LocalizationCategory.Crop, userSeed.Seed.Crop.Name, user.Language),
					emotes.GetEmote(Currency.Token.ToString()), userSeed.Seed.Crop.Price,
					_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language,
						userSeed.Seed.Crop.Price));

				if (userSeed.Seed.IsMultiply)
					seedDesc += Response.ShopSeedSeedMultiply.Parse(user.Language,
						emotes.GetEmote("Arrow"));

				if (userSeed.Seed.ReGrowth is not null)
					seedDesc += Response.ShopSeedSeedReGrowth.Parse(user.Language,
						emotes.GetEmote("Arrow"),
						timeNow.Add(userSeed.Seed.ReGrowth.Value).ToDiscordTimestamp(TimestampFormat.RelativeTime));

				embed.AddField(Response.UserFarmPlantSelectSeedsSeedTitle.Parse(user.Language,
						emotes.GetEmote(userSeed.Seed.Name),
						_local.Localize(LocalizationCategory.Seed, userSeed.Seed.Name, user.Language), userSeed.Amount),
					seedDesc + $"\n{StringExtensions.EmptyChar}");

				selectMenu.AddOption(
					_local.Localize(LocalizationCategory.Seed, userSeed.Seed.Name, user.Language),
					$"{userSeed.Seed.Id}",
					emote: Parse(emotes.GetEmote(userSeed.Seed.Name)));
			}

			var components = new ComponentBuilder()
				.WithButton(
					Response.ComponentPaginatorBack.Parse(user.Language),
					$"ufp:{page - 1}",
					disabled: page <= 1)
				.WithButton(
					Response.ComponentPaginatorForward.Parse(user.Language),
					$"ufp:{page + 1}",
					disabled: page >= maxPage)
				.WithSelectMenu(selectMenu);

			await ModifyOriginalResponseAsync(x =>
			{
				x.Embed = embed.Build();
				x.Components = components.Build();
			});
		}

		/// <summary>
		/// Step 2 - Select cells
		/// </summary>
		[ComponentInteraction("pss")]
		public async Task Execute(string[] selectedValues)
		{
			await DeferAsync(true);

			var seedId = Guid.Parse(selectedValues.First());
			var emotes = DiscordRepository.Emotes;
			var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
			var seed = await _mediator.Send(new GetSeedQuery(seedId));
			var userSeed = await _mediator.Send(new GetUserSeedQuery(user.Id, seed.Id));

			if (userSeed.Amount < 1)
			{
				throw new GameUserExpectedException(Response.UserFarmPlantNoSeeds.Parse(user.Language,
					emotes.GetEmote(Building.Farm.ToString()), emotes.GetEmote("Arrow")));
			}

			var userFarms = await _mediator.Send(new GetUserFarmsQuery(user.Id));
			var emptyFarms = userFarms
				.Where(x => x.State is FieldState.Empty)
				.ToList();

			if (emptyFarms.Any() is false)
			{
				throw new GameUserExpectedException(Response.UserFarmPlantNoEmptyCells.Parse(user.Language,
					emotes.GetEmote(Building.Farm.ToString()), emotes.GetEmote(seed.Name),
					_local.Localize(LocalizationCategory.Seed, seed.Name, user.Language)));
			}

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.UserFarmPlantAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
				.WithDescription(
					Response.UserFarmPlantSelectCellsDesc.Parse(user.Language,
						Context.User.Mention.AsGameMention(user.Title, user.Language), emotes.GetEmote(seed.Name),
						_local.Localize(LocalizationCategory.Seed, seed.Name, user.Language, 5)) +
					$"\n{StringExtensions.EmptyChar}")
				.WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Harvesting, user.Language)));

			var selectMenu = new SelectMenuBuilder()
				.WithPlaceholder(Response.ComponentUserFarmPlantSelectCells.Parse(user.Language))
				.WithCustomId($"psf:{seed.Id}")
				.WithMaxValues(userSeed.Amount >= emptyFarms.Count ? emptyFarms.Count : (int) userSeed.Amount);

			foreach (var userFarm in emptyFarms)
			{
				selectMenu.AddOption(
					Response.ComponentUserFarmPlantSelectCellsLabel.Parse(user.Language, userFarm.Number),
					$"{userFarm.Number}",
					emote: Parse(emotes.GetEmote(Building.Farm.ToString())));
			}

			var compressedSeedId = Convert.ToBase64String(seed.Id.ToByteArray());
			var components = new ComponentBuilder()
				.WithSelectMenu(selectMenu)
				.WithButton(
					Response.ComponentUserFarmPlantSelectAll.Parse(user.Language),
					$"paf:{compressedSeedId},{string.Join('.', userFarms.Select(x => x.Number))}",
					ButtonStyle.Success);

			await ModifyOriginalResponseAsync(x =>
			{
				x.Embed = embed.Build();
				x.Components = components.Build();
			});
		}

		/// <summary>
		/// Step 3 - Success (selected cells)
		/// </summary>
		[ComponentInteraction("psf:*")]
		public async Task Execute(string seedIdString, string[] selectedValues)
		{
			await DeferAsync();

			var seedId = Guid.Parse(seedIdString);
			var selectedFarms = selectedValues.Select(uint.Parse).ToArray();
			var emotes = DiscordRepository.Emotes;
			var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));
			var seed = await _mediator.Send(new GetSeedQuery(seedId));
			var userSeed = await _mediator.Send(new GetUserSeedQuery(user.Id, seed.Id));

			if (userSeed.Amount < 1)
			{
				throw new GameUserExpectedException(Response.UserFarmPlantNoSeeds.Parse(user.Language,
					emotes.GetEmote(Building.Farm.ToString()), emotes.GetEmote("Arrow")));
			}

			var userFarms = await _mediator.Send(new GetUserFarmsQuery(user.Id));

			userFarms = userFarms
				.Where(x => selectedFarms.Contains(x.Number))
				.ToList();

			if (userFarms.Any(x => x.State is not FieldState.Empty))
			{
				throw new GameUserExpectedException(Response.UserFarmPlantCellIsNotEmpty.Parse(user.Language,
					emotes.GetEmote(Building.Farm.ToString())));
			}

			await _mediator.Send(new RemoveSeedFromUserCommand(user.Id, seed.Id, (uint) selectedFarms.Length));
			await _mediator.Send(new PlantUserFarmsCommand(user.Id, selectedFarms, seed));
			await _mediator.Send(new AddStatisticToUserCommand(
				user.Id, Statistic.SeedPlanted, (uint) selectedFarms.Length));
			await _mediator.Send(new CheckAchievementsInUserCommand(user.Id, new[]
			{
				Achievement.FirstPlant,
				Achievement.Plant25Seed,
				Achievement.Plant50Seed,
				Achievement.Plant150Seed
			}));

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.UserFarmPlantAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
				.WithDescription(Response.UserFarmPlantSuccessDesc.Parse(user.Language,
					Context.User.Mention.AsGameMention(user.Title, user.Language),
					emotes.GetEmote(seed.Name), _local.Localize(LocalizationCategory.Seed, seed.Name, user.Language, 5),
					emotes.GetEmote(Building.Farm.ToString())))
				.WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Harvesting, user.Language)));

			await Context.Interaction.FollowUpResponse(embed);
			await Context.Interaction.ClearOriginalResponse(user.Language);
		}

		/// <summary>
		/// Step 3 - Success (selected all cells)
		/// </summary>
		[ComponentInteraction("paf:*,*")]
		public async Task Execute(string compressedSeedId, string selectedCellsString)
		{
			await Execute(
				new Guid(Convert.FromBase64String(compressedSeedId)).ToString(),
				selectedCellsString.Split('.'));
		}
	}
}