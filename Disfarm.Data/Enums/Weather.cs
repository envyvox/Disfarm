using System;

namespace Disfarm.Data.Enums
{
	public enum Weather : byte
	{
		Any = 0,
		Clear = 1,
		Rain = 2
	}

	public static class WeatherHelper
	{
		public static string Localize(this Weather weather, Language language)
		{
			return weather switch
			{
				Weather.Any => language switch
				{
					Language.English => "any",
					Language.Russian => "любой",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Weather.Clear => language switch
				{
					Language.English => "clear",
					Language.Russian => "ясной",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Weather.Rain => language switch
				{
					Language.English => "rainy",
					Language.Russian => "дождливой",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				_ => throw new ArgumentOutOfRangeException(nameof(weather), weather, null)
			};
		}

		public static string EmoteName(this Weather weather)
		{
			return "Weather" + weather;
		}
	}
}