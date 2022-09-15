using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Services.Game.Crop.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Disfarm.Services.Extensions.CacheExtensions;

namespace Disfarm.Services.Game.Crop.Queries
{
    public record GetCropQuery(Guid Id) : IRequest<CropDto>;

    public class GetCropHandler : IRequestHandler<GetCropQuery, CropDto>
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _db;

        public GetCropHandler(
            DbContextOptions options,
            IMapper mapper,
            IMemoryCache cache)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<CropDto> Handle(GetCropQuery request, CancellationToken ct)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.CropKey, request.Id), out CropDto crop)) return crop;

            var entity = await _db.Crops
                .Include(x => x.Seed)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (entity is null)
            {
                throw new Exception(
                    $"crop {request.Id} not found");
            }

            crop = _mapper.Map<CropDto>(entity);

            _cache.Set(string.Format(CacheExtensions.CropKey, crop.Id), crop, CacheExtensions.DefaultCacheOptions);
            
            return crop;
        }
    }
}