using System;

namespace Disfarm.Data.Enums
{
	public enum Achievement : byte
	{
		FirstFish = 1,
		FirstPlant = 2,
		FirstBet = 3,
		FirstJackpot = 4,
		FirstLotteryTicket = 5,
		FirstVendorDeal = 6,
		Catch50Fish = 7,
		Catch100Fish = 8,
		Catch300Fish = 9,
		CatchEpicFish = 10,
		CatchMythicalFish = 11,
		CatchLegendaryFish = 12,
		Plant25Seed = 13,
		Plant50Seed = 14,
		Plant150Seed = 15,
		Collect50Crop = 16,
		Collect100Crop = 17,
		Collect300Crop = 18,
		Casino33Bet = 19,
		Casino333Bet = 20,
		Casino777Bet = 21,
		Casino22LotteryBuy = 22,
		Casino99LotteryBuy = 23,
		Casino15LotteryGift = 24,
		Vendor100Sell = 25,
		Vendor777Sell = 26,
		Vendor1500Sell = 27,
		Vendor3500Sell = 28,
		CompleteCollectionFish = 29,
		CompleteCollectionCrop = 30
	}

	public static class AchievementHelper
	{
		public static string Localize(this Achievement achievement, Language language)
		{
			return achievement switch
			{
				Achievement.FirstFish => language switch
				{
					Language.English => "Catch the first fish",
					Language.Russian => "Выловить первую рыбу",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.FirstPlant => language switch
				{
					Language.English => "Plant seeds for the first time",
					Language.Russian => "Впервые посадить семена",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.FirstBet => language switch
				{
					Language.English => "Bet at the casino for the first time",
					Language.Russian => "Впервые сделать ставку в казино",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.FirstJackpot => language switch
				{
					Language.English => "Hit the jackpot for the first time in a casino",
					Language.Russian => "Впервые сорвать джек-пот в казино",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.FirstLotteryTicket => language switch
				{
					Language.English => "Buy your first lottery ticket",
					Language.Russian => "Купить свой первый лотерейный билет",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.FirstVendorDeal => language switch
				{
					Language.English => "Sell an item to a vendor for the first time",
					Language.Russian => "Впервые продать предмет скупщику",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Catch50Fish => language switch
				{
					Language.English => "Catch 50 fish",
					Language.Russian => "Выловить 50 рыб",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Catch100Fish => language switch
				{
					Language.English => "Catch 150 fish",
					Language.Russian => "Выловить 100 рыб",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Catch300Fish => language switch
				{
					Language.English => "Catch 300 fish",
					Language.Russian => "Выловить 300 рыб",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.CatchEpicFish => language switch
				{
					Language.English => "Catch epic fish",
					Language.Russian => "Поймать эпическую рыбу",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.CatchMythicalFish => language switch
				{
					Language.English => "Catch a mythical fish",
					Language.Russian => "Поймать мифическую рыбу",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.CatchLegendaryFish => language switch
				{
					Language.English => "Catch a legendary fish",
					Language.Russian => "Поймать легендарную рыбу",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Plant25Seed => language switch
				{
					Language.English => "Plant 25 seeds",
					Language.Russian => "Посадить 25 семян",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Plant50Seed => language switch
				{
					Language.English => "Plant 50 seeds",
					Language.Russian => "Посадить 50 семян",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Plant150Seed => language switch
				{
					Language.English => "Plant 150 seeds",
					Language.Russian => "Посадить 150 семян",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Collect50Crop => language switch
				{
					Language.English => "Collect 50 crops",
					Language.Russian => "Собрать 50 урожая",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Collect100Crop => language switch
				{
					Language.English => "Collect 100 crops",
					Language.Russian => "Собрать 100 урожая",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Collect300Crop => language switch
				{
					Language.English => "Collect 300 crops",
					Language.Russian => "Собрать 300 урожая",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Casino33Bet => language switch
				{
					Language.English => "Make 33 bets",
					Language.Russian => "Сделать 33 ставки",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Casino333Bet => language switch
				{
					Language.English => "Make 333 bets",
					Language.Russian => "Сделать 333 ставки",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Casino777Bet => language switch
				{
					Language.English => "Make 777 bets",
					Language.Russian => "Сделать 777 ставок",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Casino22LotteryBuy => language switch
				{
					Language.English => "Buy 22 lottery tickets",
					Language.Russian => "Купить 22 лотерейных билета",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Casino99LotteryBuy => language switch
				{
					Language.English => "Buy 99 lottery tickets",
					Language.Russian => "Купить 99 лотерейных билетов",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Casino15LotteryGift => language switch
				{
					Language.English => "Gift 15 lottery tickets",
					Language.Russian => "Подарить 15 лотерейных билетов",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Vendor100Sell => language switch
				{
					Language.English => "Sell 100 items to a vendor",
					Language.Russian => "Продать скупщику 100 предметов",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Vendor777Sell => language switch
				{
					Language.English => "Sell 777 items to a vendor",
					Language.Russian => "Продать скупщику 777 предметов",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Vendor1500Sell => language switch
				{
					Language.English => "Sell 1500 items to a vendor",
					Language.Russian => "Продать скупщику 1500 предметов",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.Vendor3500Sell => language switch
				{
					Language.English => "Sell 3500 items to a vendor",
					Language.Russian => "Продать скупщику 3500 предметов",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.CompleteCollectionFish => language switch
				{
					Language.English => "Collect a complete collection of fish",
					Language.Russian => "Собрать полную коллекцию рыбы",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				Achievement.CompleteCollectionCrop => language switch
				{
					Language.English => "Collect a complete collection of crops",
					Language.Russian => "Собрать полную коллекцию урожая",
					_ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
				},
				_ => throw new ArgumentOutOfRangeException(nameof(achievement), achievement, null)
			};
		}

		public static AchievementCategory Category(this Achievement achievement)
		{
			return achievement switch
			{
				Achievement.FirstFish => AchievementCategory.FirstSteps,
				Achievement.FirstPlant => AchievementCategory.FirstSteps,
				Achievement.FirstBet => AchievementCategory.FirstSteps,
				Achievement.FirstJackpot => AchievementCategory.FirstSteps,
				Achievement.FirstLotteryTicket => AchievementCategory.FirstSteps,
				Achievement.FirstVendorDeal => AchievementCategory.FirstSteps,
				Achievement.Catch50Fish => AchievementCategory.Fishing,
				Achievement.Catch100Fish => AchievementCategory.Fishing,
				Achievement.Catch300Fish => AchievementCategory.Fishing,
				Achievement.CatchEpicFish => AchievementCategory.Fishing,
				Achievement.CatchMythicalFish => AchievementCategory.Fishing,
				Achievement.CatchLegendaryFish => AchievementCategory.Fishing,
				Achievement.Plant25Seed => AchievementCategory.Harvesting,
				Achievement.Plant50Seed => AchievementCategory.Harvesting,
				Achievement.Plant150Seed => AchievementCategory.Harvesting,
				Achievement.Collect50Crop => AchievementCategory.Harvesting,
				Achievement.Collect100Crop => AchievementCategory.Harvesting,
				Achievement.Collect300Crop => AchievementCategory.Harvesting,
				Achievement.Casino33Bet => AchievementCategory.Casino,
				Achievement.Casino333Bet => AchievementCategory.Casino,
				Achievement.Casino777Bet => AchievementCategory.Casino,
				Achievement.Casino22LotteryBuy => AchievementCategory.Casino,
				Achievement.Casino99LotteryBuy => AchievementCategory.Casino,
				Achievement.Casino15LotteryGift => AchievementCategory.Casino,
				Achievement.Vendor100Sell => AchievementCategory.Trading,
				Achievement.Vendor777Sell => AchievementCategory.Trading,
				Achievement.Vendor1500Sell => AchievementCategory.Trading,
				Achievement.Vendor3500Sell => AchievementCategory.Trading,
				Achievement.CompleteCollectionFish => AchievementCategory.Collection,
				Achievement.CompleteCollectionCrop => AchievementCategory.Collection,
				_ => throw new ArgumentOutOfRangeException(nameof(achievement), achievement, null)
			};
		}
	}
}