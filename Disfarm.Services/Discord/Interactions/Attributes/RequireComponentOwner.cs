using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Discord.Interactions.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	public class RequireComponentOwner : PreconditionAttribute
	{
		public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context,
			ICommandInfo commandInfo, IServiceProvider services)
		{
			var service = services.GetRequiredService<IMediator>();
			var user = await service.Send(new GetUserQuery((long)context.User.Id));
			var param = (context as SocketInteractionContext)?.SegmentMatches.First().Value;

			if (ulong.TryParse(param, out var id))
			{
				return context.User.Id != id
					? PreconditionResult.FromError(Response.ComponentOwnerOnly.Parse(user.Language))
					: PreconditionResult.FromSuccess();
			}

			return PreconditionResult.FromError("Parse cannot be done if no user ID exists.");
		}
	}
}