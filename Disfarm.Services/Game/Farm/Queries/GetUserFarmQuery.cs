using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Farm.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Farm.Queries
{
	public record GetUserFarmQuery(long UserId, uint Number) : IRequest<UserFarmDto>;

	public class GetUserFarmHandler : IRequestHandler<GetUserFarmQuery, UserFarmDto>
	{
		private readonly IMapper _mapper;
		private readonly AppDbContext _db;

		public GetUserFarmHandler(
			DbContextOptions options,
			IMapper mapper)
		{
			_db = new AppDbContext(options);
			_mapper = mapper;
		}

		public async Task<UserFarmDto> Handle(GetUserFarmQuery request, CancellationToken cancellationToken)
		{
			var entity = await _db.UserFarms
				.Include(x => x.Seed)
				.ThenInclude(x => x.Crop)
				.Where(x =>
					x.UserId == request.UserId &&
					x.Number == request.Number)
				.SingleOrDefaultAsync();

			if (entity is null)
			{
				throw new Exception(
					$"User {request.UserId} doesnt have farm with number {request.Number}");
			}

			return _mapper.Map<UserFarmDto>(entity);
		}
	}
}