using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Emote.Extensions;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Discord.Interactions.Attributes
{
    public class RequireLocation : PreconditionAttribute
    {
        private readonly Location _requiredLocation;

        public RequireLocation(Location requiredLocation)
        {
            _requiredLocation = requiredLocation;
        }

        public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context,
            ICommandInfo commandInfo, IServiceProvider services)
        {
            var emotes = DiscordRepository.Emotes;
            var service = services.GetRequiredService<IMediator>();
            var user = await service.Send(new GetUserQuery((long) context.User.Id));

            return user.Location == _requiredLocation
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError(
                    user.Location switch
                    {
                        Location.Fishing =>
                            Response.PreconditionRequireLocationButYouFishing.Parse(user.Language),
                        Location.FarmWatering =>
                            Response.PreconditionRequireLocationButYouFarmWatering.Parse(user.Language),
                        Location.WorkOnContract =>
                            Response.PreconditionRequireLocationButYouWorkOnContract.Parse(user.Language),
                        _ =>
                            Response.PreconditionRequireLocationButYouAnotherLocation.Parse(user.Language,
                                _requiredLocation.Localize(user.Language, true), emotes.GetEmote("DiscordSlashCommand"))
                    });
        }
    }
}