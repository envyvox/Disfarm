using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Crop.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Crop.Queries
{
	public record GetUserCropQuery(long UserId, Guid CropId) : IRequest<UserCropDto>;

	public class GetUserCropHandler : IRequestHandler<GetUserCropQuery, UserCropDto>
	{
		private readonly IMapper _mapper;
		private readonly AppDbContext _db;

		public GetUserCropHandler(
			DbContextOptions options,
			IMapper mapper)
		{
			_db = new AppDbContext(options);
			_mapper = mapper;
		}

		public async Task<UserCropDto> Handle(GetUserCropQuery request, CancellationToken ct)
		{
			var entity = await _db.UserCrops
				.Include(x => x.Crop)
				.ThenInclude(x => x.Seed)
				.SingleOrDefaultAsync(x =>
					x.UserId == request.UserId &&
					x.CropId == request.CropId);

			return entity is null
				? new UserCropDto(Guid.Empty, null, 0, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
				: _mapper.Map<UserCropDto>(entity);
		}
	}
}