using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Discord.Image.Queries
{
    public record GetImageUrlQuery(Data.Enums.Image Type, Language Language) : IRequest<string>;

    public class GetImageUrlHandler : IRequestHandler<GetImageUrlQuery, string>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public GetImageUrlHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<string> Handle(GetImageUrlQuery request, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var requestedImageUrl = await db.Images
                .OrderByRandom()
                .Where(x =>
                    x.Type == request.Type &&
                    x.Language == request.Language)
                .Take(1)
                .Select(x => x.Url)
                .SingleOrDefaultAsync();

            if (requestedImageUrl is not null) return requestedImageUrl;

            var placeholderUrl = await db.Images
                .AsQueryable()
                .Where(x =>
                    x.Type == Data.Enums.Image.Placeholder &&
                    x.Language == request.Language)
                .Select(x => x.Url)
                .SingleOrDefaultAsync();

            if (placeholderUrl is null)
            {
                throw new Exception(
                    $"there is no urls for image {request.Type.ToString()} with language {request.Language.ToString()} or even {Data.Enums.Image.Placeholder.ToString()} in db");
            }

            return placeholderUrl;
        }
    }
}