using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Game.User.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Discord.Interactions.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class RequireGuildContext : PreconditionAttribute
	{
		public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context,
			ICommandInfo commandInfo, IServiceProvider services)
		{
			var user = await services.GetRequiredService<IMediator>()
				.Send(new GetUserQuery((long) context.User.Id));

			return context.Interaction.IsDMInteraction
				? PreconditionResult.FromError(Response.RequireGuildContext.Parse(user.Language))
				: PreconditionResult.FromSuccess();
		}
	}
}