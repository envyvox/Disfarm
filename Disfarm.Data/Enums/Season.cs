using System;

namespace Disfarm.Data.Enums
{
    public enum Season : byte
    {
        Any = 0,
        Spring = 1,
        Summer = 2,
        Autumn = 3,
        Winter = 4
    }

    public static class SeasonHelper
    {
        public static string Localize(this Season season, Language language, bool declension = false)
        {
            return season switch
            {
                Season.Any => language switch
                {
                    Language.English => declension ? "" : "Any",
                    Language.Russian => declension ? "" : "Любой",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Season.Spring => language switch
                {
                    Language.English => declension ? "spring" : "Spring",
                    Language.Russian => declension ? "весны" : "Весна",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Season.Summer => language switch
                {
                    Language.English => declension ? "summer" : "Summer",
                    Language.Russian => declension ? "лета" : "Лето",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Season.Autumn => language switch
                {
                    Language.English => declension ? "autumn" : "Autumn",
                    Language.Russian => declension ? "осени" : "Осень",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Season.Winter => language switch
                {
                    Language.English => declension ? "winter" : "Winter",
                    Language.Russian => declension ? "зимы" : "Зима",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(season), season, null)
            };
        }

        public static string EmoteName(this Season season)
        {
            return "Season" + season;
        }

        public static Season NextSeason(this Season season)
        {
            return season.GetHashCode() is 4 ? Season.Spring : (Season) season.GetHashCode() + 1;
        }
    }
}