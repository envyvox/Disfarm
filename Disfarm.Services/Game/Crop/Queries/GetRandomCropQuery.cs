using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Extensions;
using Disfarm.Services.Game.Crop.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Crop.Queries
{
    public record GetRandomCropQuery : IRequest<CropDto>;

    public class GetRandomCropHandler : IRequestHandler<GetRandomCropQuery, CropDto>
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetRandomCropHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<CropDto> Handle(GetRandomCropQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.Crops
                .OrderByRandom()
                .FirstOrDefaultAsync();

            return _mapper.Map<CropDto>(entity);
        }
    }
}