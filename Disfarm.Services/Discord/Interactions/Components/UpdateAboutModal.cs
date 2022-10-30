using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.User.Commands;
using Disfarm.Services.Game.User.Queries;
using MediatR;

namespace Disfarm.Services.Discord.Interactions.Components
{
	public class UpdateAboutModal : IModal
	{
		public string Title => "Change profile about";

		[InputLabel("test")]
		[RequiredInput(false)]
		[ModalTextInput("user-about", TextInputStyle.Paragraph, maxLength: 1024)]
		public string About { get; set; }
	}

	public class UserProfileUpdateAboutModal : InteractionModuleBase<SocketInteractionContext>
	{
		private readonly IMediator _mediator;

		public UserProfileUpdateAboutModal(IMediator mediator)
		{
			_mediator = mediator;
		}

		[ComponentInteraction("user-profile-update-about")]
		public async Task Execute()
		{
			var user = await _mediator.Send(new GetUserQuery((long)Context.User.Id));

			await Context.Interaction.RespondWithModalAsync<UpdateAboutModal>("user-profile-update-about-modal", null,
				x => x
					.WithTitle(Response.ComponentUpdateAboutModalTitle.Parse(user.Language))
					.UpdateTextInput("user-about", builder => builder
						.WithLabel(Response.ComponentUserProfileUpdateAboutLabel.Parse(user.Language))
						.WithPlaceholder(Response.ComponentUpdateAboutModalAbout.Parse(user.Language))
						.WithValue(user.About)));
		}

		[ModalInteraction("user-profile-update-about-modal")]
		public async Task Execute(UpdateAboutModal modal)
		{
			await DeferAsync();

			var user = await _mediator.Send(new GetUserQuery((long)Context.User.Id));

			modal.About = modal.About is "" ? null : modal.About;

			await _mediator.Send(new UpdateUserCommand(user with { About = modal.About }));

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithAuthor(Response.UserProfileUpdateAboutAuthor.Parse(user.Language), Context.User.GetAvatarUrl())
				.WithDescription(Response.UserProfileUpdateAboutDesc.Parse(user.Language,
					Context.User.Mention.AsGameMention(user.Title, user.Language)));

			await Context.Interaction.FollowUpResponse(embed);
		}
	}
}