using System;
using System.Collections.Generic;
using System.Linq;
using Discord.Interactions;

namespace Disfarm.Data.Enums
{
    public enum AchievementCategory : byte
    {
        [ChoiceDisplay("First steps")] FirstSteps = 1,
        Fishing = 2,
        Harvesting = 3,
        Casino = 4,
        Trading = 5,
        Collection = 6
    }

    public static class AchievementCategoryHelper
    {
        public static string Localize(this AchievementCategory category, Language language)
        {
            return category switch
            {
                AchievementCategory.FirstSteps => language switch
                {
                    Language.English => "First steps",
                    Language.Russian => "Первые шаги",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                AchievementCategory.Fishing => language switch
                {
                    Language.English => "Fishing",
                    Language.Russian => "Рыбалка",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                AchievementCategory.Harvesting => language switch
                {
                    Language.English => "Harvesting",
                    Language.Russian => "Выращивание урожая",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                AchievementCategory.Casino => language switch
                {
                    Language.English => "Casino",
                    Language.Russian => "Казино",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                AchievementCategory.Trading => language switch
                {
                    Language.English => "Trading",
                    Language.Russian => "Торговля",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                AchievementCategory.Collection => language switch
                {
                    Language.English => "Collection",
                    Language.Russian => "Коллекция",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            };
        }

        public static IEnumerable<Achievement> Achievements(this AchievementCategory category)
        {
            return Enum
                .GetValues(typeof(Achievement))
                .Cast<Achievement>()
                .Where(x => x.Category() == category);
        }
    }
}