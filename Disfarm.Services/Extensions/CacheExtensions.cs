using System;
using Microsoft.Extensions.Caching.Memory;

namespace Disfarm.Services.Extensions
{
    public static class CacheExtensions
    {
        public const string XpRequired = "level_{0}_required_xp";
        public const string WorldStateKey = "world_state";

        public const string FishesKey = "fishes";
        public const string FishesWithSeasonKey = "fishes_season_{0}";
        public const string FishIdKey = "fish_id_{0}";
        public const string FishNameKey = "fish_name_{0}";

        public const string SeedsKey = "seeds";
        public const string SeedsWithSeasonKey = "seed_season_{0}";
        public const string SeedKey = "seed_{0}";

        public const string CropsKey = "crops";
        public const string CropIdKey = "crop_id_{0}";
        public const string CropNameKey = "crop_name_{0}";

        public const string AchievementKey = "achievement_{0}";
        public const string AchievementsKey = "achievements_category_{0}";
        public const string UserHasAchievementKey = "user_{0}_has_achievement_{1}";
        
        public const string LocalizationsInCategory = "localizations_category_{0}_language_{1}";


        public static readonly MemoryCacheEntryOptions DefaultCacheOptions =
            new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
    }
}