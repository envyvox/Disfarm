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
    public record GetCropByNameQuery(string Name) : IRequest<CropDto>;

    public class GetCropByNameHandler : IRequestHandler<GetCropByNameQuery, CropDto>
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;

        public GetCropByNameHandler(
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            IMemoryCache cache)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<CropDto> Handle(GetCropByNameQuery request, CancellationToken ct)
        {
            if (_cache.TryGetValue(CacheExtensions.GetCropByNameKey(request.Name), out CropDto crop))
                return crop;

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = await db.Crops
                .SingleOrDefaultAsync(x => x.Name == request.Name);

            if (entity is null)
            {
                throw new Exception(
                    $"crop with name {request.Name} not found");
            }

            crop = _mapper.Map<CropDto>(entity);

            _cache.Set(CacheExtensions.GetCropByNameKey(request.Name), crop,
                CacheExtensions.DefaultCacheOptions);

            return crop;
        }
    }
}