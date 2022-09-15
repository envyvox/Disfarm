using System;

namespace Disfarm.Data.Enums
{
    public enum FishRarity : byte
    {
        Common = 1,
        Rare = 2,
        Epic = 3,
        Mythical = 4,
        Legendary = 5
    }

    public static class FishRarityHelper
    {
        public static string Localize(this FishRarity rarity, Language language, bool declension = false)
        {
            return rarity switch
            {
                FishRarity.Common => language switch
                {
                    Language.English => declension ? "common" : "Common",
                    Language.Russian => declension ? "обычную" : "Обычная",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                FishRarity.Rare => language switch
                {
                    Language.English => declension ? "rare" : "Rare",
                    Language.Russian => declension ? "редкую" : "Редкая",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                FishRarity.Epic => language switch
                {
                    Language.English => declension ? "epic" : "Epic",
                    Language.Russian => declension ? "эпическую" : "Эпическая",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                FishRarity.Mythical => language switch
                {
                    Language.English => declension ? "mythical" : "Mythical",
                    Language.Russian => declension ? "мифическую" : "Мифическая",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                FishRarity.Legendary => language switch
                {
                    Language.English => declension ? "legendary" : "Legendary",
                    Language.Russian => declension ? "легендарную" : "Легендарная",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };
        }
    }
}