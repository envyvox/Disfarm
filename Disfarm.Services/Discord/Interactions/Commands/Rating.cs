﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data;
using Disfarm.Data.Entities.User;
using Disfarm.Data.Enums;
using Disfarm.Data.Util;
using Disfarm.Services.Discord.Embed;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Discord.Emote.Models;
using Disfarm.Services.Discord.Guild.Queries;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Discord.Interactions.Commands
{
    [Group("rating", "View game ratings")]
    public class Rating : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly ILocalizationService _local;
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;

        public Rating(
            DbContextOptions options,
            ILocalizationService local,
            IMediator mediator)
        {
            _db = new AppDbContext(options);
            _local = local;
            _mediator = mediator;
        }

        [SlashCommand("tokens", "User rating by tokens")]
        public async Task ExecuteTokens()
        {
            await DeferAsync();

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

            var entities = await _db.UserCurrencies
                .Include(x => x.User)
                .Where(x =>
                    x.Type == Currency.Token &&
                    x.Amount > 0)
                .OrderByDescending(x => x.Amount)
                .Take(10)
                .ToListAsync();

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.RatingTokensAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Rating, user.Language)));

            if (entities.Count > 0)
            {
                for (var pos = 1; pos <= entities.Count; pos++)
                {
                    var current = entities[pos - 1];
                    var socketUser = await _mediator.Send(new GetSocketGuildUserQuery(
                        Context.Guild.Id, (ulong) current.User.Id));
                    var mention = socketUser is null
                        ? $"{emotes.GetEmote(current.User.Title.EmoteName())} {current.User.Title.Localize(user.Language)} <@{current.User.Id}>"
                        : socketUser.Mention.AsGameMention(current.User.Title, user.Language);

                    embed.AddField(StringExtensions.EmptyChar,
                        $"{pos.AsPositionEmote()} `{pos}` {mention} {emotes.GetEmote("Arrow")} " +
                        $"{emotes.GetEmote(Currency.Token.ToString())} {current.Amount} " +
                        $"{_local.Localize(LocalizationCategory.Currency, Currency.Token.ToString(), user.Language, current.Amount)}");
                }
            }
            else
            {
                embed.AddField(StringExtensions.EmptyChar, Response.RatingEmpty.Parse(user.Language));
            }

            await _mediator.Send(new FollowUpEmbedCommand(Context.Interaction, embed));
        }

        [SlashCommand("expirience", "User rating by expirience")]
        public async Task ExecuteExpirience()
        {
            await DeferAsync();

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

            var entities = await _db.Users
                .AsQueryable()
                .OrderByDescending(x => x.Xp)
                .Take(10)
                .ToListAsync();

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.RatingXpAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Rating, user.Language)));

            if (entities.Count > 0)
            {
                for (var pos = 1; pos <= entities.Count; pos++)
                {
                    var current = entities[pos - 1];
                    var socketUser = await _mediator.Send(new GetSocketGuildUserQuery(
                        Context.Guild.Id, (ulong) current.Id));
                    var mention = socketUser is null
                        ? $"{emotes.GetEmote(current.Title.EmoteName())} {current.Title.Localize(user.Language)} <@{current.Id}>"
                        : socketUser.Mention.AsGameMention(current.Title, user.Language);

                    embed.AddField(StringExtensions.EmptyChar, Response.RatingXpFieldDesc.Parse(user.Language,
                        pos.AsPositionEmote(), pos, mention, emotes.GetEmote("Arrow"), current.Level.AsLevelEmote(),
                        current.Level, emotes.GetEmote("Xp"), current.Xp));
                }
            }
            else
            {
                embed.AddField(StringExtensions.EmptyChar, Response.RatingEmpty.Parse(user.Language));
            }

            await _mediator.Send(new FollowUpEmbedCommand(Context.Interaction, embed));
        }

        [SlashCommand("achievements", "User rating by achievements")]
        public async Task ExecuteAchievements()
        {
            await DeferAsync();

            var emotes = DiscordRepository.Emotes;
            var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

            var entities = _db.UserAchievements
                .Include(x => x.User)
                .Include(x => x.Achievement)
                .AsEnumerable()
                .GroupBy(x => x.UserId)
                .ToList();

            var users = entities.ToDictionary(
                x => x.First().User,
                x => (uint) x.Sum(y => y.Achievement.Points));

            var embed = new EmbedBuilder()
                .WithUserColor(user.CommandColor)
                .WithAuthor(Response.RatingAchievementsAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.Rating, user.Language)));

            if (users.Count > 0)
            {
                var pos = 1;
                foreach (var (current, points) in users)
                {
                    var socketUser = await _mediator.Send(new GetSocketGuildUserQuery(
                        Context.Guild.Id, (ulong) current.Id));
                    var mention = socketUser is null
                        ? $"{emotes.GetEmote(current.Title.EmoteName())} {current.Title.Localize(user.Language)} <@{current.Id}>"
                        : socketUser.Mention.AsGameMention(current.Title, user.Language);

                    embed.AddField(StringExtensions.EmptyChar, Response.RatingAchievementsFieldDesc.Parse(user.Language,
                        pos.AsPositionEmote(), pos, mention, emotes.GetEmote("Arrow"), emotes.GetEmote("Achievement"),
                        points));

                    pos++;
                }
            }
            else
            {
                embed.AddField(StringExtensions.EmptyChar, Response.RatingEmpty.Parse(user.Language));
            }

            await _mediator.Send(new FollowUpEmbedCommand(Context.Interaction, embed));
        }
    }
}