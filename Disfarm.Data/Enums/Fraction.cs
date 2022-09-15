using System;

namespace Disfarm.Data.Enums
{
    public enum Fraction : byte
    {
        Neutral = 0,
        RedRose = 1,
        WhiteCrow = 2,
        GoldenShark = 3
    }

    public static class FractionHelper
    {
        public static string Localize(this Fraction fraction, Language language)
        {
            return fraction switch
            {
                Fraction.Neutral => language switch
                {
                    Language.English => "Neutral",
                    Language.Russian => "Нейтрал",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Fraction.RedRose => language switch
                {
                    Language.English => "«Red rose»",
                    Language.Russian => "«Алая роза»",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Fraction.WhiteCrow => language switch
                {
                    Language.English => "«White crow»",
                    Language.Russian => "«Белый ворон»",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Fraction.GoldenShark => language switch
                {
                    Language.English => "«Golden shark»",
                    Language.Russian => "«Золотая акула»",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(fraction), fraction, null)
            };
        }

        public static string EmoteName(this Fraction fraction)
        {
            return "Fraction" + fraction;
        }

        public static string Description(this Fraction fraction, Language language)
        {
            return fraction switch
            {
                Fraction.RedRose => language switch
                {
                    Language.English =>
                        "Сформировав крепкие связи благодаря своим любовным гостиницам, розы способны убедить любого в своей правоте. Никогда не знаешь через кого они выходят на нужных людей, однако своих целей они достигают быстро и красиво.",
                    Language.Russian =>
                        "Сформировав крепкие связи благодаря своим любовным гостиницам, розы способны убедить любого в своей правоте. Никогда не знаешь через кого они выходят на нужных людей, однако своих целей они достигают быстро и красиво.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Fraction.WhiteCrow => language switch
                {
                    Language.English =>
                        "Отшельники, предпочитающие находится подальше от шумной **Нейтральной зоны**, и проворачивать свои дела без лишних глаз. Не ведут никаких дел с другими фракциями и нейтралами, благодаря чему о них практически ничего не известно.",
                    Language.Russian =>
                        "Отшельники, предпочитающие находится подальше от шумной **Нейтральной зоны**, и проворачивать свои дела без лишних глаз. Не ведут никаких дел с другими фракциями и нейтралами, благодаря чему о них практически ничего не известно.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Fraction.GoldenShark => language switch
                {
                    Language.English =>
                        "Шумная жизнь, огромное количество денег и пропорционально растущее недоверие ко всем вокруг. Если бы не деньги, никто не стал бы сотрудничать с акулами, однако деньги есть деньги.",
                    Language.Russian =>
                        "Шумная жизнь, огромное количество денег и пропорционально растущее недоверие ко всем вокруг. Если бы не деньги, никто не стал бы сотрудничать с акулами, однако деньги есть деньги.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(fraction), fraction, null)
            };
        }
    }
}