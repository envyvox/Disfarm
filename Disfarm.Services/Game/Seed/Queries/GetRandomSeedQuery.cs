using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Seed.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Seed.Queries
{
	public record GetRandomSeedQuery : IRequest<SeedDto>;

	public class GetRandomSeedHandler : IRequestHandler<GetRandomSeedQuery, SeedDto>
	{
		private readonly IMapper _mapper;
		private readonly AppDbContext _db;

		public GetRandomSeedHandler(
			DbContextOptions options,
			IMapper mapper)
		{
			_db = new AppDbContext(options);
			_mapper = mapper;
		}

		public async Task<SeedDto> Handle(GetRandomSeedQuery request, CancellationToken ct)
		{
			var entity = await _db.Seeds
				.OrderByRandom()
				.Include(x => x.Crop)
				.FirstOrDefaultAsync();

			return _mapper.Map<SeedDto>(entity);
		}
	}
}