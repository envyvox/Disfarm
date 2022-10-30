using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Discord.Image.Queries;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.User.Commands;
using Disfarm.Services.Game.User.Queries;
using MediatR;

namespace Disfarm.Services.Discord.Interactions.Components.UserTitles
{
	public class UserTitlesUpdate : InteractionModuleBase<SocketInteractionContext>
	{
		private readonly IMediator _mediator;

		public UserTitlesUpdate(IMediator mediator)
		{
			_mediator = mediator;
		}

		[ComponentInteraction("user-title-update")]
		public async Task Execute(string[] selectedValues)
		{
			await DeferAsync();

			var title = (Title)int.Parse(selectedValues.First());
			var user = await _mediator.Send(new GetUserQuery((long)Context.User.Id));

			await _mediator.Send(new UpdateUserCommand(user with { Title = title }));

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.UserTitlesUpdateAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
				.WithDescription(
					Response.UserTitlesUpdateDesc.Parse(user.Language,
						Context.User.Mention.AsGameMention(title, user.Language)))
				.WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Data.Enums.Image.UserTitles, user.Language)));

			await Context.Interaction.FollowUpResponse(embed);
			await Context.Interaction.ClearOriginalResponse(user.Language);
		}
	}
}