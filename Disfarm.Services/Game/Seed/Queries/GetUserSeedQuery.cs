using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Seed.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Seed.Queries
{
	public record GetUserSeedQuery(long UserId, Guid SeedId) : IRequest<UserSeedDto>;

	public class GetUserSeedHandler : IRequestHandler<GetUserSeedQuery, UserSeedDto>
	{
		private readonly IMapper _mapper;
		private readonly AppDbContext _db;

		public GetUserSeedHandler(
			DbContextOptions options,
			IMapper mapper)
		{
			_db = new AppDbContext(options);
			_mapper = mapper;
		}

		public async Task<UserSeedDto> Handle(GetUserSeedQuery request, CancellationToken ct)
		{
			var entity = await _db.UserSeeds
				.Include(x => x.Seed)
				.SingleOrDefaultAsync(x =>
					x.UserId == request.UserId &&
					x.SeedId == request.SeedId);

			return entity is null
				? new UserSeedDto(Guid.Empty, null, 0, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
				: _mapper.Map<UserSeedDto>(entity);
		}
	}
}