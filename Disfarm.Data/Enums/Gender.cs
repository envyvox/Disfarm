using System;

namespace Disfarm.Data.Enums
{
    public enum Gender : byte
    {
        None = 0,
        Male = 1,
        Female = 2
    }

    public static class GenderHelper
    {
        public static string Localize(this Gender gender, Language language)
        {
            return gender switch
            {
                Gender.None => language switch
                {
                    Language.English => "Not specified",
                    Language.Russian => "Не указан",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Gender.Male => language switch
                {
                    Language.English => "Male",
                    Language.Russian => "Мужской",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Gender.Female => language switch
                {
                    Language.English => "Female",
                    Language.Russian => "Женский",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
            };
        }

        public static string EmoteName(this Gender gender)
        {
            return "Gender" + gender;
        }
    }
}