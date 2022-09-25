using System;

namespace Disfarm.Data.Enums
{
    public enum Location : byte
    {
        InTransit = 0,
        Neutral = 1,
        Fishing = 2,
        WorkOnContract = 3,
        FarmWatering = 4
    }

    public static class LocationHelper
    {
        public static string Localize(this Location location, Language language, bool declension = false)
        {
            return location switch
            {
                // not displayed
                Location.InTransit => language switch
                {
                    Language.English => "",
                    Language.Russian => "",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Location.Neutral => language switch
                {
                    Language.English => declension ? "Idle in Neutral zone" : "Neutral zone",
                    Language.Russian => declension ? "Бездействие в Нейтральной зоне" : "Нейтральная зона",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Location.Fishing => language switch
                {
                    Language.English => declension ? "fishing" : "Fishing",
                    Language.Russian => declension ? "рыбалке" : "Рыбалка",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                // not displayed
                Location.WorkOnContract => language switch
                {
                    Language.English => "",
                    Language.Russian => "",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Location.FarmWatering => language switch
                {
                    Language.English => declension ? "farm watering" : "Farm watering",
                    Language.Russian => declension ? "поливке фермы" : "Поливка фермы",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
            };
        }
        
        public static string EmoteName(this Location location)
        {
            return location switch
            {
                Location.InTransit => "InTransit",
                Location.Neutral => Fraction.Neutral.EmoteName(),
                Location.Fishing => "Fishing",
                Location.WorkOnContract => "WorkOnContract",
                Location.FarmWatering => "FarmWatering",
                _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
            };
        }
    }
}