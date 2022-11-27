using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Product.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Product.Queries
{
	public record GetUserProductsQuery(long UserId) : IRequest<List<UserProductDto>>;

	public class GetUserProductsHandler : IRequestHandler<GetUserProductsQuery, List<UserProductDto>>
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly IMapper _mapper;

		public GetUserProductsHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
		{
			_scopeFactory = scopeFactory;
			_mapper = mapper;
		}

		public async Task<List<UserProductDto>> Handle(GetUserProductsQuery request, CancellationToken ct)
		{
			using var scope = _scopeFactory.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			var entities = await db.UserProducts
				.Include(x => x.Product)
				.Where(x => x.UserId == request.UserId)
				.ToListAsync();

			return _mapper.Map<List<UserProductDto>>(entities);
		}
	}
}