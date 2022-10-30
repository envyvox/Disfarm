using System;

namespace Disfarm.Data.Enums
{
	public enum FieldState : byte
	{
		Empty = 0,
		Planted = 1,
		Watered = 2,
		Completed = 3
	}

	public static class FieldStateHelper
	{
		public static string Localize(this FieldState state, Language language) => state switch
		{
			FieldState.Empty => language switch
			{
				Language.English => "Empty",
				Language.Russian => "Пустая",
				_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
			},
			FieldState.Planted => language switch
			{
				Language.English => "Planted",
				Language.Russian => "Засажена",
				_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
			},
			FieldState.Watered => language switch
			{
				Language.English => "Watered",
				Language.Russian => "Полита",
				_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
			},
			FieldState.Completed => language switch
			{
				Language.English => "Ready to collect",
				Language.Russian => "Готово к сбору",
				_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
			},
			_ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
		};
	}
}