using System;

namespace Disfarm.Data.Enums
{
    public enum BannerRarity : byte
    {
        Common = 1,
        Rare = 2,
        Animated = 3,
        Limited = 4,
        Custom = 5
    }

    public static class BannerRarityHelper
    {
        public static string Localize(this BannerRarity rarity, Language language, bool declension = false)
        {
            return rarity switch
            {
                BannerRarity.Common => language switch
                {
                    Language.English => declension ? "common" : "Common",
                    Language.Russian => declension ? "обычного" : "Обычный",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                BannerRarity.Rare => language switch
                {
                    Language.English => declension ? "rare" : "Rare",
                    Language.Russian => declension ? "редкого" : "Редкий",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                BannerRarity.Animated => language switch
                {
                    Language.English => declension ? "animated" : "Animated",
                    Language.Russian => declension ? "анимированного" : "Анимированный",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                BannerRarity.Limited => language switch
                {
                    Language.English => declension ? "limited" : "Limited",
                    Language.Russian => declension ? "лимитированного" : "Лимитированный",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                BannerRarity.Custom => language switch
                {
                    Language.English => declension ? "custom" : "Custom",
                    Language.Russian => declension ? "персонального" : "Персональный",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };
        }

        public static string EmoteName(this BannerRarity rarity)
        {
            return "BannerRarity" + rarity;
        }
    }
}