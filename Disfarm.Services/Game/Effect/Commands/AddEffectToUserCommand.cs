using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Disfarm.Data;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using Disfarm.Services.Discord.Client.Queries;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Currency.Commands;
using Disfarm.Services.Game.Effect.Queries;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Statistic.Commands;
using Disfarm.Services.Game.World.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Image = Disfarm.Data.Enums.Image;

namespace Disfarm.Services.Game.Effect.Commands
{
    public record AddEffectToUserCommand(
            long UserId,
            Data.Enums.Effect Effect,
            TimeSpan? Duration)
        : IRequest;

    public class AddEffectToUserHandler : IRequestHandler<AddEffectToUserCommand>
    {
        private readonly ILogger<AddEffectToUserHandler> _logger;
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private readonly AppDbContext _db;

        public AddEffectToUserHandler(
            DbContextOptions options,
            ILogger<AddEffectToUserHandler> logger,
            IMediator mediator,
            ILocalizationService local)
        {
            _db = new AppDbContext(options);
            _logger = logger;
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(AddEffectToUserCommand request, CancellationToken ct)
        {
            var entity = await AsyncEnumerable.SingleOrDefaultAsync(_db.UserEffects, x =>
                x.UserId == request.UserId &&
                x.Type == request.Effect);

            if (entity is null)
            {
                var created = await _db.CreateEntity(new UserEffect
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Type = request.Effect,
                    Expiration = request.Duration is null
                        ? null
                        : DateTimeOffset.UtcNow.Add((TimeSpan) request.Duration),
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                });

                _logger.LogInformation(
                    "Created user effect entity {@Entity}",
                    created);
            }
            else
            {
                entity.Expiration = entity.Expiration is null
                    ? request.Duration is null ? null : DateTimeOffset.UtcNow.Add((TimeSpan) request.Duration)
                    : request.Duration is null ? null : entity.Expiration?.Add((TimeSpan) request.Duration);
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                await _db.UpdateEntity(entity);

                _logger.LogInformation(
                    "Added user {UserId} effect {Effect} for {Duration}",
                    request.UserId, request.Effect.ToString(), request.Duration);
            }

            if (request.Effect is Data.Enums.Effect.Lottery) await CheckLottery();

            return Unit.Value;
        }

        private async Task CheckLottery()
        {
            var lotteryUsers = await _mediator.Send(new CountUsersWithEffectQuery(
                Data.Enums.Effect.Lottery));
            var requiredUsers = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldProperty.CasinoLotteryRequiredUsers));

            if (lotteryUsers >= requiredUsers) await StartLottery();
        }

        private async Task StartLottery()
        {
            var emotes = DiscordRepository.Emotes;
            var lotteryAward = await _mediator.Send(new GetWorldPropertyValueQuery(
                WorldProperty.CasinoLotteryAward));

            var winner = await _db.UserEffects
                .OrderByRandom()
                .Include(x => x.User)
                .Where(x => x.Type == Data.Enums.Effect.Lottery)
                .Select(x => x.User)
                .FirstOrDefaultAsync();

            await _mediator.Send(new AddCurrencyToUserCommand(winner.Id, Data.Enums.Currency.Token, lotteryAward));
            await _mediator.Send(new AddStatisticToUserCommand(winner.Id, Data.Enums.Statistic.CasinoLotteryWin));
            await _mediator.Send(new RemoveEffectFromUsersCommand(Data.Enums.Effect.Lottery));

            var socketUser = await _mediator.Send(new GetClientUserQuery((ulong) winner.Id));
            var embed = new EmbedBuilder()
                .WithUserColor(winner.CommandColor)
                .WithAuthor(Response.LotteryWinAuthor.Parse(winner.Language), socketUser.GetAvatarUrl())
                .WithDescription(Response.LotteryWinDesc.Parse(winner.Language,
                    socketUser.Mention.AsGameMention(winner.Title, winner.Language), emotes.GetEmote("LotteryTicket"),
                    emotes.GetEmote(Data.Enums.Currency.Token.ToString()), lotteryAward,
                    _local.Localize(LocalizationCategory.Currency, Data.Enums.Currency.Token.ToString(),
                        winner.Language, lotteryAward)))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Casino, winner.Language)));

            await _mediator.Send(new SendEmbedToUserCommand(socketUser.Id, embed));
        }
    }
}