using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Entities;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.World.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Disfarm.Services.Game.World.Commands
{
	public record UpdateWorldStateQuery(WorldStateDto WorldState) : IRequest;

	public class UpdateWorldStateHandler : IRequestHandler<UpdateWorldStateQuery>
	{
		private readonly IMapper _mapper;
		private readonly ILogger<UpdateWorldStateHandler> _logger;
		private readonly AppDbContext _db;

		public UpdateWorldStateHandler(
			DbContextOptions options,
			IMapper mapper,
			ILogger<UpdateWorldStateHandler> logger)
		{
			_db = new AppDbContext(options);
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<Unit> Handle(UpdateWorldStateQuery request, CancellationToken ct)
		{
			var updated = await _db.UpdateEntity(_mapper.Map<WorldState>(request.WorldState with
			{
				UpdatedAt = DateTimeOffset.UtcNow
			}));

			_logger.LogInformation(
				"Updated world state entity {@Entity}",
				updated);

			return Unit.Value;
		}
	}
}