using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Product.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Product.Queries
{
	public record GetProductQuery(Guid Id) : IRequest<ProductDto>;

	public class GetProductHandler : IRequestHandler<GetProductQuery, ProductDto>
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly IMapper _mapper;

		public GetProductHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
		{
			_scopeFactory = scopeFactory;
			_mapper = mapper;
		}

		public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken ct)
		{
			using var scope = _scopeFactory.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			var entity = await db.Products.SingleOrDefaultAsync(x => x.Id == request.Id);

			if (entity is null)
			{
				throw new Exception($"product with id {request.Id} not found");
			}

			return _mapper.Map<ProductDto>(entity);
		}
	}
}