using System;
using Microsoft.Extensions.Caching.Memory;

namespace Disfarm.Services.Extensions
{
    public class CacheExtensions
    {
        public const string XpRequired = "level_{0}_required_xp";
        public const string WorldStateKey = "world_state";

        public const string FishesKey = "fishes";
        public const string FishesWithSeasonKey = "fishes_season_{0}";
        public const string FishKey = "fish_{0}";

        public const string SeedsKey = "seeds";
        public const string SeedsWithSeasonKey = "seed_season_{0}";
        public const string SeedKey = "seed_{0}";

        public const string CropsKey = "crops";
        public const string CropKey = "crop_{0}";


        public static readonly MemoryCacheEntryOptions DefaultCacheOptions =
            new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
    }
}