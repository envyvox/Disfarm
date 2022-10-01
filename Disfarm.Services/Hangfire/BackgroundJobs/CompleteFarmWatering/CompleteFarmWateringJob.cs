using System.Threading.Tasks;
using Discord;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Guild.Queries;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Farm.Commands;
using Disfarm.Services.Game.Transit.Commands;
using Disfarm.Services.Game.User.Commands;
using Disfarm.Services.Game.User.Queries;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Image = Disfarm.Data.Enums.Image;

namespace Disfarm.Services.Hangfire.BackgroundJobs.CompleteFarmWatering
{
    public class CompleteFarmWateringJob : ICompleteFarmWateringJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CompleteFarmWateringJob> _logger;

        public CompleteFarmWateringJob(
            IMediator mediator,
            ILogger<CompleteFarmWateringJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Execute(ulong guildId, ulong channelId, long userId, uint farmCount)
        {
            _logger.LogInformation(
                "Complete farm watering job executed for user {UserId}",
                userId);

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery(userId));
            var socketUser = await _mediator.Send(new GetSocketGuildUserQuery(guildId, (ulong) user.Id));
            var xpFarmWatering = await _mediator.Send(new GetWorldPropertyValueQuery(WorldProperty.XpFarmWatering));

            await _mediator.Send(new UpdateUserCommand(user with {Location = Location.Neutral}));
            await _mediator.Send(new AddXpToUserCommand(guildId, channelId, userId, xpFarmWatering * farmCount));
            await _mediator.Send(new DeleteUserMovementCommand(user.Id));
            await _mediator.Send(new UpdateUserFarmsStateCommand(user.Id, FieldState.Watered));

            var embed = new EmbedBuilder()
                .WithAuthor(Response.UserFarmWaterAuthor.Parse(user.Language), socketUser.GetAvatarUrl())
                .WithDescription(Response.UserFarmWaterCompleted.Parse(user.Language,
                    socketUser.Mention.AsGameMention(user.Title, user.Language),
                    emotes.GetEmote(Building.Farm.ToString()), emotes.GetEmote("Arrow"), emotes.GetEmote("Xp"),
                    xpFarmWatering * farmCount))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Harvesting, user.Language)));

            await _mediator.Send(new SendEmbedToUserCommand(guildId, channelId, socketUser.Id, embed));
        }
    }
}