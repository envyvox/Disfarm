using System;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Disfarm.Services.Game.Localization.Impl
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public LocalizationService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public string Localize(LocalizationCategory category, string keyword, Language language, uint amount = 1)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var entity = db.Localizations
                .SingleOrDefaultAsync(x =>
                    x.Category == category &&
                    x.Name == keyword &&
                    x.Language == language)
                .Result;

            if (entity is null)
            {
                throw new Exception(
                    $"localization with category {category.ToString()} and keyword {keyword} not found");
            }

            return entity.Localize(amount);
        }
    }
}