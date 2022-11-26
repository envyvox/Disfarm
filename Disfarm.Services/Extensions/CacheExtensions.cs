using System;
using Disfarm.Data.Enums;
using Disfarm.Data.Enums.Achievement;
using Microsoft.Extensions.Caching.Memory;

namespace Disfarm.Services.Extensions
{
	public static class CacheExtensions
	{
		public static string GetXpRequiredKey(uint level) =>
			$"level_{level}_required_xp";

		public static string GetWorldStateKey() =>
			"world_state";

		public static string GetFishesKey() =>
			"fishes";

		public static string GetFishesWithSeasonKey(Season season) =>
			$"fishes_season_{season.GetHashCode()}";

		public static string GetFishByIdKey(Guid id) =>
			$"fish_id_{id}";

		public static string GetFishByNameKey(string name) =>
			$"fish_name_{name}";

		public static string GetSeedsKey() =>
			"seeds";

		public static string GetSeedsWithSeasonKey(Season season) =>
			$"seed_season_{season.GetHashCode()}";

		public static string GetSeedByIdKey(Guid id) =>
			$"seed_id_{id}";

		public static string GetCropsKey() =>
			"crops";

		public static string GetCropByIdKey(Guid id) =>
			$"crop_id_{id}";

		public static string GetCropByNameKey(string name) =>
			$"crop_name_{name}";

		public static string GetAchievementKey(Achievement achievement) =>
			$"achievement_{achievement.GetHashCode()}";

		public static string GetAchievementsInCategoryKey(AchievementCategory category) =>
			$"achievements_category_{category.GetHashCode()}";

		public static string GetUserHasAchievementKey(long userId, Achievement achievement) =>
			$"user_{userId}_has_achievement_{achievement.GetHashCode()}";

		public static string GetLocalizationsInCategoryKey(LocalizationCategory category, Language language) =>
			$"localizations_category_{category.GetHashCode()}_language_{language.GetHashCode()}";


		public static readonly MemoryCacheEntryOptions DefaultCacheOptions =
			new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
	}
}