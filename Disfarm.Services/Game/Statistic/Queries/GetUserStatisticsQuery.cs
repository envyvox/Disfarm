using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Statistic.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Statistic.Queries
{
	public record GetUserStatisticsQuery(long UserId) : IRequest<List<UserStatisticDto>>;

	public class GetUserStatisticsHandler : IRequestHandler<GetUserStatisticsQuery, List<UserStatisticDto>>
	{
		private readonly IMapper _mapper;
		private readonly AppDbContext _db;

		public GetUserStatisticsHandler(
			DbContextOptions options,
			IMapper mapper)
		{
			_db = new AppDbContext(options);
			_mapper = mapper;
		}

		public async Task<List<UserStatisticDto>> Handle(GetUserStatisticsQuery request, CancellationToken ct)
		{
			var entities = await _db.UserStatistics
				.AsQueryable()
				.Where(x => x.UserId == request.UserId)
				.ToListAsync();

			return _mapper.Map<List<UserStatisticDto>>(entities);
		}
	}
}