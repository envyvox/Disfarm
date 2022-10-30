using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.User.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Referral.Queries
{
	public record GetUserReferralsQuery(long UserId) : IRequest<List<UserDto>>;

	public class GetUserReferralsHandler : IRequestHandler<GetUserReferralsQuery, List<UserDto>>
	{
		private readonly IMapper _mapper;
		private readonly AppDbContext _db;

		public GetUserReferralsHandler(
			DbContextOptions options,
			IMapper mapper)
		{
			_db = new AppDbContext(options);
			_mapper = mapper;
		}

		public async Task<List<UserDto>> Handle(GetUserReferralsQuery request, CancellationToken ct)
		{
			var entities = await _db.UserReferrers
				.Include(x => x.User)
				.Where(x => x.ReferrerId == request.UserId)
				.Select(x => x.User)
				.ToListAsync();

			return _mapper.Map<List<UserDto>>(entities);
		}
	}
}