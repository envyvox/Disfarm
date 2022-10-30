using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.Effect.Commands
{
	public record RemoveEffectFromUsersCommand(Data.Enums.Effect Effect) : IRequest;

	public class RemoveEffectFromUsersHandler : IRequestHandler<RemoveEffectFromUsersCommand>
	{
		private readonly ILogger<RemoveEffectFromUsersHandler> _logger;
		private readonly AppDbContext _db;

		public RemoveEffectFromUsersHandler(
			DbContextOptions options,
			ILogger<RemoveEffectFromUsersHandler> logger)
		{
			_db = new AppDbContext(options);
			_logger = logger;
		}

		public async Task<Unit> Handle(RemoveEffectFromUsersCommand request, CancellationToken ct)
		{
			await _db.Database.ExecuteSqlInterpolatedAsync(@$"
                delete from user_effects
                where type = {request.Effect};");

			_logger.LogInformation(
				"Deleted user effect entities where effect {Effect}",
				request.Effect.ToString());

			return Unit.Value;
		}
	}
}