using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using MediatR;

namespace Disfarm.Services.Discord.Client.Queries
{
	public record GetClientUserQuery(ulong UserId) : IRequest<IUser>;

	public class GetClientUserHandler : IRequestHandler<GetClientUserQuery, IUser>
	{
		private readonly IDiscordClientService _discordClientService;

		public GetClientUserHandler(IDiscordClientService discordClientService)
		{
			_discordClientService = discordClientService;
		}

		public async Task<IUser> Handle(GetClientUserQuery request, CancellationToken ct)
		{
			var client = await _discordClientService.GetSocketClient();
			var user = await client.GetUserAsync(request.UserId);

			if (user is null)
			{
				throw new Exception(
					$"user with id {request.UserId} not found in client");
			}

			return user;
		}
	}
}