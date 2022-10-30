using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Referral.Queries
{
	public record GetUserReferralCountQuery(long UserId) : IRequest<uint>;

	public class GetUserReferralCountHandler : IRequestHandler<GetUserReferralCountQuery, uint>
	{
		private readonly AppDbContext _db;

		public GetUserReferralCountHandler(DbContextOptions options)
		{
			_db = new AppDbContext(options);
		}

		public async Task<uint> Handle(GetUserReferralCountQuery request, CancellationToken ct)
		{
			return (uint)await _db.UserReferrers
				.CountAsync(x => x.ReferrerId == request.UserId);
		}
	}
}