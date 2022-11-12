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
using Disfarm.Services.Game.Achievement.Commands;
using Disfarm.Services.Game.Collection.Commands;
using Disfarm.Services.Game.Crop.Commands;
using Disfarm.Services.Game.Farm.Commands;
using Disfarm.Services.Game.Farm.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Statistic.Commands;
using Disfarm.Services.Game.User.Commands;
using Disfarm.Services.Game.User.Queries;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using static Disfarm.Services.Extensions.ExceptionExtensions;

namespace Disfarm.Services.Discord.Interactions.Components.Farm
{
	[RequireLocation(Location.Neutral)]
	public class UserFarmCollect : InteractionModuleBase<SocketInteractionContext>
	{
		private readonly IMediator _mediator;
		private readonly ILocalizationService _local;
		private readonly TimeZoneInfo _timeZoneInfo;
		private readonly Random _random = new();

		public UserFarmCollect(
			IMediator mediator,
			ILocalizationService local,
			TimeZoneInfo timeZoneInfo)
		{
			_mediator = mediator;
			_local = local;
			_timeZoneInfo = timeZoneInfo;
		}

		[ComponentInteraction("user-farm-collect")]
		public async Task Execute()
		{
			await DeferAsync();

			var timeNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _timeZoneInfo);
			var emotes = DiscordRepository.Emotes;
			var user = await _mediator.Send(new GetUserQuery((long)Context.User.Id));
			var userFarms = await _mediator.Send(new GetUserFarmsQuery(user.Id));

			userFarms = userFarms
				.Where(x => x.State is FieldState.Completed)
				.ToList();

			if (userFarms.Any() is false)
			{
				throw new GameUserExpectedException(Response.UserFarmCollectNoCompletedCells.Parse(user.Language,
					emotes.GetEmote(Building.Farm.ToString())));
			}

			var xpAmount = await _mediator.Send(new GetWorldPropertyValueQuery(
				WorldProperty.XpCropHarvesting));

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.UserFarmCollectAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
				.WithDescription(
					Response.UserFarmCollectDesc.Parse(user.Language,
						Context.User.Mention.AsGameMention(user.Title, user.Language),
						emotes.GetEmote(Building.Farm.ToString())) +
					$"\n{StringExtensions.EmptyChar}")
				.WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Harvesting, user.Language)));

			foreach (var userFarm in userFarms)
			{
				var amount = userFarm.Seed.IsMultiply
					? (uint)_random.Next(2, 4)
					: 1;

				await _mediator.Send(new AddCropToUserCommand(user.Id, userFarm.Seed.Crop.Id, amount));
				await _mediator.Send(new AddCollectionToUserCommand(
					user.Id, CollectionCategory.Crop, userFarm.Seed.Crop.Id));

				var desc = Response.UserFarmCellDesc.Parse(user.Language,
					emotes.GetEmote(userFarm.Seed.Crop.Name), amount,
					_local.Localize(LocalizationCategory.Crop, userFarm.Seed.Crop.Name, user.Language, amount),
					emotes.GetEmote("Xp"), xpAmount);

				if (userFarm.Seed.ReGrowth is not null)
				{
					await _mediator.Send(new StartReGrowthOnUserFarmCommand(user.Id, userFarm.Number));

					desc += Response.UserFarmCellReGrowth.Parse(user.Language,
						emotes.GetEmote("Arrow"),
						timeNow.Add(userFarm.Seed.ReGrowth.Value).ToDiscordTimestamp(TimestampFormat.RelativeTime));
				}
				else
				{
					await _mediator.Send(new ResetUserFarmCommand(user.Id, userFarm.Number));

					desc += Response.UserFarmCellEmpty.Parse(user.Language,
						emotes.GetEmote("Arrow"), emotes.GetEmote(Building.Farm.ToString()));
				}

				embed.AddField(Response.UserFarmCellTitle.Parse(user.Language,
						emotes.GetEmote("List"), emotes.GetEmote(Building.Farm.ToString()), userFarm.Number),
					desc);
			}

			await _mediator.Send(new AddStatisticToUserCommand(
				user.Id, Statistic.CropHarvested, (uint)userFarms.Count));
			await _mediator.Send(new AddXpToUserCommand(user.Id, xpAmount * (uint)userFarms.Count));
			await _mediator.Send(new CheckAchievementsInUserCommand(user.Id, new[]
			{
				Achievement.Collect50Crop,
				Achievement.Collect100Crop,
				Achievement.Collect300Crop,
				Achievement.CompleteCollectionCrop
			}));

			await Context.Interaction.FollowUpResponse(embed);
			await Context.Interaction.ClearOriginalResponse(user.Language);
		}
	}
}