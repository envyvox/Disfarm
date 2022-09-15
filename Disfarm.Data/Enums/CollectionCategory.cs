using System;

namespace Disfarm.Data.Enums
{
    public enum CollectionCategory : byte
    {
        Crop = 1,
        Fish = 2
    }

    public static class CollectionHelper
    {
        public static string Localize(this CollectionCategory category, Language language) => category switch
        {
            CollectionCategory.Crop => language switch
            {
                Language.English => "Crop",
                Language.Russian => "Урожай",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            CollectionCategory.Fish => language switch
            {
                Language.English => "Fish",
                Language.Russian => "Рыба",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}