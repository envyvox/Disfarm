using System;

namespace Disfarm.Data.Enums
{
    public enum TimesDayType : byte
    {
        Any = 0,
        Day = 1,
        Night = 2
    }

    public static class TimesDayHelper
    {
        public static string Localize(this TimesDayType timesDay, Language language) => timesDay switch
        {
            TimesDayType.Any => language switch
            {
                Language.English => "any",
                Language.Russian => "любое",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            TimesDayType.Day => language switch
            {
                Language.English => "day",
                Language.Russian => "день",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            TimesDayType.Night => language switch
            {
                Language.English => "night",
                Language.Russian => "ночь",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(timesDay), timesDay, null)
        };
    }
}