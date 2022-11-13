using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Crop.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Crop.Queries
{
    public record GetCropQuery(Guid Id) : IRequest<CropDto>;

    public class GetCropHandler : IRequestHandler<GetCropQuery, CropDto>
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetCropHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            IMemoryCache cache)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<CropDto> Handle(GetCropQuery request, CancellationToken ct)
        {
            if (_cache.TryGetValue(CacheExtensions.GetCropByIdKey(request.Id), out CropDto crop)) return crop;

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.Crops
                .Include(x => x.Seed)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception(
                    $"crop {request.Id} not found");
            }

            crop = _mapper.Map<CropDto>(entity);

            _cache.Set(CacheExtensions.GetCropByIdKey(request.Id), crop, CacheExtensions.DefaultCacheOptions);

            return crop;
        }
    }
}