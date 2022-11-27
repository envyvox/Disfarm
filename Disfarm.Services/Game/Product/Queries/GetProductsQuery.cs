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
	public record GetProductsQuery : IRequest<List<ProductDto>>;

	public class GetProductsHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly IMapper _mapper;

		public GetProductsHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
		{
			_scopeFactory = scopeFactory;
			_mapper = mapper;
		}

		public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken ct)
		{
			using var scope = _scopeFactory.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			var entities = await db.Products
				.AsQueryable()
				.OrderByDescending(x => x.Id)
				.ToListAsync();

			return _mapper.Map<List<ProductDto>>(entities);
		}
	}
}