using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Disfarm.Data.Enums;
using Disfarm.Services.Discord.Interactions.Attributes;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.User.Queries;
using MediatR;

namespace Disfarm.Services.Discord.Interactions.Commands
{
	[RequireGuildContext]
	public class Help : InteractionModuleBase<SocketInteractionContext>
	{
		private readonly IMediator _mediator;
		private const int MaxPage = 5;

		public Help(IMediator mediator)
		{
			_mediator = mediator;
		}

		[SlashCommand("help", "Learn about the possibilities of the bot")]
		public async Task Execute() => await Execute("1");

		[ComponentInteraction("help-paginator:*")]
		public async Task Execute(string pageString)
		{
			await DeferAsync(true);
			var page = int.Parse(pageString);

			var user = await _mediator.Send(new GetUserQuery((long) Context.User.Id));

			var embed = new EmbedBuilder()
				.WithUserColor(user.CommandColor)
				.WithFooter(Response.PaginatorFooter.Parse(user.Language, page, MaxPage));

			switch (page)
			{
				case 1:
				{
					embed
						.WithAuthor("Title", Context.User.GetAvatarUrl())
						.WithDescription("...");
					break;
				}
				case 2:
				{
					embed
						.WithAuthor("Title", Context.User.GetAvatarUrl())
						.WithDescription("...");
					break;
				}
				case 3:
				{
					embed
						.WithAuthor("Title", Context.User.GetAvatarUrl())
						.WithDescription("...");
					break;
				}
				case 4:
				{
					embed
						.WithAuthor("Title", Context.User.GetAvatarUrl())
						.WithDescription("...");
						
					break;
				}
				case 5:
				{
					embed
						.WithAuthor("Title", Context.User.GetAvatarUrl())
						.WithDescription("...");
					break;
				}
			}

			var components = new ComponentBuilder()
				.WithButton(
					Response.ComponentPaginatorBack.Parse(user.Language),
					$"help-paginator:{page - 1}",
					disabled: page <= 1)
				.WithButton(
					Response.ComponentPaginatorForward.Parse(user.Language),
					$"help-paginator:{page + 1}",
					disabled: page >= MaxPage);

			await ModifyOriginalResponseAsync(x =>
			{
				x.Embed = embed.Build();
				x.Components = components.Build();
			});
		}
	}
}