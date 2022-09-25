﻿using System;

namespace Disfarm.Data.Enums
{
    public enum Title : byte
    {
        Newbie = 1,
        BelieverInLuck = 2,
        KingExcitement = 3,
        LuckBringer = 4,
        FirstSamurai = 5
    }

    public static class TitleHelper
    {
        public static string Localize(this Title title, Language language)
        {
            return title switch
            {
                Title.Newbie => language switch
                {
                    Language.English => "Newbie",
                    Language.Russian => "Новичок",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Title.BelieverInLuck => language switch
                {
                    Language.English => "Believer in luck",
                    Language.Russian => "Верящий в удачу",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Title.KingExcitement => language switch
                {
                    Language.English => "King of excitement",
                    Language.Russian => "Король азарта",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Title.LuckBringer => language switch
                {
                    Language.English => "Bringer of luck",
                    Language.Russian => "Приносящий удачу",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Title.FirstSamurai => language switch
                {
                    Language.English => "First samurai",
                    Language.Russian => "Первый самурай",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(title), title, null)
            };
        }

        public static string EmoteName(this Title title)
        {
            return "Title" + title;
        }
    }
}