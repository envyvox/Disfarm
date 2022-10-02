using System;

namespace Disfarm.Data.Enums
{
    public enum Response
    {
        // embeds
        UserProfileAuthor,
        UserProfileGenderTitle,
        UserProfileLevelTitle,
        UserProfileLevelDescLevel,
        UserProfileLevelDescNextLevel,
        UserProfileCreatedAtTitle,
        UserProfileAboutTitle,
        UserProfileAboutDesc,
        UserProfileFractionTitle,
        UserInventoryAuthor,
        UserInventoryNoCategoryDesc,
        UserInventoryFishesCategoryDesc,
        UserInventorySeedsCategoryDesc,
        UserInventoryCropsCategoryDesc,
        UserInventoryCurrencyTitle,
        UserInventoryContainersTitle,
        UserInventoryFishesTitle,
        UserInventorySeedsTitle,
        UserInventoryCropsTitle,
        UserInventoryTooMuchFishes,
        UserInventoryTooMuchSeeds,
        UserInventoryTooMuchCrops,
        UserInventoryCategoryEmpty,
        SettingsLanguageTitle,
        SettingsLanguageDesc,
        WorldStateAuthor,
        WorldStateDesc,
        WorldStateTimesDayDesc,
        WorldStateWeatherTodayDesc,
        WorldStateWeatherTomorrowDesc,
        WorldStateSeasonDesc,
        WorldInfoQaTimesDayAuthor,
        WorldInfoQaTimesDayDesc,
        WorldInfoQaWeatherAuthor,
        WorldInfoQaWeatherDesc,
        WorldInfoQaSeasonAuthor,
        WorldInfoQaSeasonDesc,
        UserTitlesAuthor,
        UserTitlesDesc,
        UserTitleCurrentTitle,
        PaginatorFooter,
        UserTitlesTitleDesc,
        UserTitlesUpdateAuthor,
        UserTitlesUpdateDesc,
        UserProfileUpdateAboutAuthor,
        UserProfileUpdateAboutDesc,
        UserBannersAuthor,
        UserBannersDesc,
        UserBannersCurrentBanner,
        UserBannersBannerDesc,
        UserBannersUpdateAuthor,
        UserBannersUpdateDesc,
        FishingDesc,
        FishingExpectedRewardTitle,
        FishingExpectedRewardDesc,
        WillEndTitle,
        CubeDropPressButton,
        CubeDropWaiting,
        CubeDrops,
        CompleteFishingSuccessDesc,
        CompleteFishingFailDesc,
        CompleteFishingRewardTitle,
        CompleteFishingRewardSuccessDesc,
        CompleteFishingRewardFailDesc,
        UserCollectionAuthor,
        UserCollectionDesc,
        UserCollectionCropString,
        UserCollectionCropSummer,
        UserCollectionCropAutumn,
        VendorAuthor,
        VendorDesc,
        VendorItemPrice,
        VendorBulkSellDesc,
        VendorBulkSellResultTitle,
        VendorBulkSellResultMoney,
        VendorBulkSellResultTooLong,
        UserProfileLocationTitle,
        UserProfileLocationTransit,
        UserProfileLocationFishingAndFarmWatering,
        ShopBannerAuthor,
        ShopBannerDesc,
        ShopBannerCurrentCurrencyTitle,
        ShopBannerBannerDesc,
        ShopBannerBuyAuthor,
        ShopBannerBuyDesc,
        ShopSeedAuthor,
        ShopSeedDesc,
        ShopSeedSeedDesc,
        ShopSeedSeedMultiply,
        ShopSeedSeedReGrowth,
        ShopSeedSeedPrice,
        ShopSeedBuyAuthor,
        ShopSeedBuyDesc,
        UserFarmAuthor,
        UserFarmDesc,
        UserFarmFieldEmptyTitle,
        UserFarmFieldEmptyDesc,
        UserFarmFieldPlantedTitle,
        UserFarmFieldPlantedDesc,
        UserFarmFieldWateredTitle,
        UserFarmFieldWateredDesc,
        UserFarmFieldCompletedTitle,
        UserFarmFieldCompletedDesc,
        UserFarmFieldCompletedReGrowthDesc,
        UserFarmNeedToBuyDesc,
        UserFarmBuyAuthor,
        UserFarmBuyDesc,
        UserFarmQaHarvestingAuthor,
        UserFarmQaHarvestingDesc,
        UserFarmQaUpgradingAuthor,
        UserFarmQaUpgradingDesc,
        UserFarmCollectAuthor,
        UserFarmCollectDesc,
        UserFarmCellTitle,
        UserFarmCellDesc,
        UserFarmCellReGrowth,
        UserFarmCellEmpty,
        UserFarmUpgradeAuthor,
        UserFarmUpgradeDesc,
        UserFarmWaterAuthor,
        UserFarmWaterDesc,
        UserFarmWaterCompleted,
        UserFarmDigAuthor,
        UserFarmDigDesc,
        UserFarmDigCompleted,
        UserFarmPlantAuthor,
        UserFarmPlantSelectSeedsDesc,
        UserFarmPlantSelectSeedsSeedTitle,
        UserFarmPlantSelectCellsDesc,
        UserFarmPlantSuccessDesc,
        LevelUpRewardAuthor,
        LevelUpRewardDesc,
        LevelUpRewardTitle,
        LevelUpRewardChips,
        LevelUpRewardBanner,
        LevelUpRewardContainer,
        HowCubeDropWorksAuthor,
        HowCubeDropWorksDesc,
        AchievementAuthor,
        AchievementDesc,
        AchievementRewardChip,
        AchievementRewardTitle,
        UserAchievementsAuthor,
        UserAchievementsDesc,
        UserAchievementsAchievementDescChip,
        UserAchievementsAchievementDescTitle,
        UserAchievementsAchievementCompleted,
        RatingTokensAuthor,
        RatingAchievementsAuthor,
        RatingAchievementsFieldDesc,
        RatingXpAuthor,
        RatingXpFieldDesc,
        RatingEmpty,
        ContainerOpenAuthor,
        ContainerOpenTokenDesc,
        VendorSellDesc,
        SendCurrencyAuthor,
        SendCurrencyDesc,
        SendCurrencyNotifyDesc,

        // components
        ComponentUserProfileUpdateAboutLabel,
        ComponentUserProfileUpdateCommandColor,
        ComponentContainerOpenTokens,
        ComponentContainerOpenSupplies,
        ComponentTimesDayQa,
        ComponentWeatherQa,
        ComponentSeasonQa,
        ComponentPaginatorBack,
        ComponentPaginatorForward,
        ComponentUserTitleUpdate,
        ComponentUpdateAboutModalTitle,
        ComponentUpdateAboutModalAboutLabel,
        ComponentUpdateAboutModalAbout,
        ComponentUserBannerUpdate,
        ComponentCubeDrop,
        ComponentCubeDropHowWorks,
        ComponentVendorSellFish,
        ComponentVendorSellCrops,
        ComponentShopBannerSelectCurrencyToken,
        ComponentShopBannerSelectCurrencyChip,
        ComponentShopBannerSelectBanner,
        ComponentShopSeedBuy,
        ComponentUserFarmPlant,
        ComponentUserFarmWater,
        ComponentUserFarmCollect,
        ComponentUserFarmDig,
        ComponentUserFarmQaHarvesting,
        ComponentUserFarmQaUpgrading,
        ComponentUserFarmBuy,
        ComponentFarmUpgrade,
        ComponentUserFarmDigSelect,
        ComponentUserFarmDigSelectLabel,
        ComponentUserFarmDigSelectDesc,
        ComponentUserFarmPlantSelectSeed,
        ComponentUserFarmPlantSelectCells,
        ComponentUserFarmPlantSelectCellsLabel,
        ComponentOpenExecutedChannel,

        // exceptions
        SomethingWentWrongTitle,
        SomethingWentWrongDesc,
        SettingsLanguageAlready,
        PreconditionRequireLocationButYouFishing,
        PreconditionRequireLocationButYouFarmWatering,
        PreconditionRequireLocationButYouWorkOnContract,
        PreconditionRequireLocationButYouAnotherLocation,
        ComponentOwnerOnly,
        VendorBulkSellFishNothing,
        VendorBulkSellCropsNothing,
        ShopBannerBuyNoCurrency,
        ShopSeedBuyNoCurrency,
        UserFarmBuyNoCurrency,
        UserFarmCollectNoCompletedCells,
        UserFarmUpgradeNoCurrency,
        UserFarmWaterNoPlatedCells,
        UserFarmPlantNoSeeds,
        UserFarmPlantNoEmptyCells,
        UserFarmPlantCellIsNotEmpty,
        ContainerOpenNoContainers,
        VendorSellDontHaveThatMuch,
        SendCurrencyNoCurrency
    }

    public static class ResponseHandler
    {
        private static string Localize(this Response response, Language language)
        {
            return response switch
            {
                Response.SomethingWentWrongTitle => language switch
                {
                    Language.English =>
                        "Oops, looks like something went wrong...",
                    Language.Russian =>
                        "Ой, кажется что-то пошло не так...",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.SomethingWentWrongDesc => language switch
                {
                    Language.English =>
                        "{0}, something unusual happened and I already reported it to the development team. " +
                        "I apologize for my dummy creators, they will definitely improve.",
                    Language.Russian =>
                        "{0}, произошло что-то необычное и я уже сообщила об этом команде разработки. " +
                        "Приношу извинения за моих глупых создателей, они обязательно исправятся.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserProfileAuthor => language switch
                {
                    Language.English => "Profile",
                    Language.Russian => "Профиль",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserProfileGenderTitle => language switch
                {
                    Language.English => "Gender",
                    Language.Russian => "Пол",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserProfileLevelTitle => language switch
                {
                    Language.English => "Level",
                    Language.Russian => "Уровень",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserProfileLevelDescLevel => language switch
                {
                    Language.English => "{0} {1} level, {2} {3} exp",
                    Language.Russian => "{0} {1} уровень, {2} {3} ед. опыта",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserProfileLevelDescNextLevel => language switch
                {
                    Language.English => " {0} you need {1} {2} exp to reach the next level",
                    Language.Russian => " {0} до следующего уровня необходимо еще {1} {2} ед. опыта",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserProfileCreatedAtTitle => language switch
                {
                    Language.English => "Created At",
                    Language.Russian => "Дата присоединения",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserProfileAboutTitle => language switch
                {
                    Language.English => "About",
                    Language.Russian => "Информация",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserProfileAboutDesc => language switch
                {
                    Language.English => "There is nothing specified here yet, but I'm sure this is a great user.",
                    Language.Russian => "Тут пока что ничего не указано, но я уверена что это отличный пользователь.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserProfileUpdateAboutLabel => language switch
                {
                    Language.English => "Update about",
                    Language.Russian => "Изменить информацию",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserProfileUpdateCommandColor => language switch
                {
                    Language.English => "Update command color",
                    Language.Russian => "Изменить цвет команд",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserProfileFractionTitle => language switch
                {
                    Language.English => "Fraction",
                    Language.Russian => "Фракция",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventoryTooMuchFishes => language switch
                {
                    Language.English =>
                        "You have too many fish, type </inventory:0> and select a **fish category** to view it",
                    Language.Russian =>
                        "У тебя слишком много рыбы, напиши </inventory:0> и выбери **категорию рыбы**, чтобы посмотреть ее",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventoryTooMuchSeeds => language switch
                {
                    Language.English =>
                        "You have too many seeds, type </inventory:0> and select a **seeds category** to view it",
                    Language.Russian =>
                        "У тебя слишком много семян, напиши </inventory:0> и выбери **категорию семян**, чтобы посмотреть их",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventoryTooMuchCrops => language switch
                {
                    Language.English =>
                        "You have too many crops, type </inventory:0> and select a **crops category** to view it",
                    Language.Russian =>
                        "У тебя слишком много урожая, напиши </inventory:0> и выбери **категорию урожая**, чтобы посмотреть его",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventoryCategoryEmpty => language switch
                {
                    Language.English =>
                        "You don't have any items of this type",
                    Language.Russian =>
                        "У тебя нет ни одного предмета этого типа",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventoryAuthor => language switch
                {
                    Language.English => "Inventory",
                    Language.Russian => "Инвентарь",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventoryNoCategoryDesc => language switch
                {
                    Language.English => "all received items go here:",
                    Language.Russian => "все полученные предметы попадают сюда:",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventoryFishesCategoryDesc => language switch
                {
                    Language.English => "your fish is displayed here:",
                    Language.Russian => "тут отображается твоя рыба:",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventorySeedsCategoryDesc => language switch
                {
                    Language.English => "your seeds is displayed here:",
                    Language.Russian => "тут отображается твои семена:",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventoryCropsCategoryDesc => language switch
                {
                    Language.English => "your crops is displayed here:",
                    Language.Russian => "тут отображается твой урожай:",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventoryCurrencyTitle => language switch
                {
                    Language.English => "Currency",
                    Language.Russian => "Валюта",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventoryContainersTitle => language switch
                {
                    Language.English => "Containers",
                    Language.Russian => "Контейнеры",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventoryFishesTitle => language switch
                {
                    Language.English => "Fish",
                    Language.Russian => "Рыба",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventorySeedsTitle => language switch
                {
                    Language.English => "Seeds",
                    Language.Russian => "Семена",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventoryCropsTitle => language switch
                {
                    Language.English => "Crops",
                    Language.Russian => "Урожай",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentContainerOpenTokens => language switch
                {
                    Language.English => "Open containers with tokens",
                    Language.Russian => "Открыть контейнеры с токенами",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentContainerOpenSupplies => language switch
                {
                    Language.English => "Open containers with supplies",
                    Language.Russian => "Открыть контейнеры с припасами",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.SettingsLanguageTitle => language switch
                {
                    Language.English => "Bot language update",
                    Language.Russian => "Обновление языка бота",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.SettingsLanguageDesc => language switch
                {
                    Language.English => "{0}, you have successfully updated the language of the bot.",
                    Language.Russian => "{0}, ты успешно обновил язык бота.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.SettingsLanguageAlready => language switch
                {
                    Language.English => "the specified language is already set as the language of the bot.",
                    Language.Russian => "указанный язык уже установлен как язык бота.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.WorldStateAuthor => language switch
                {
                    Language.English => "World Information",
                    Language.Russian => "Информация о мире",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.WorldStateDesc => language switch
                {
                    Language.English => "{0}, here is information about the current state of the world:",
                    Language.Russian => "{0}, тут отображается информация о текущем состоянии мира:",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.WorldStateWeatherTodayDesc => language switch
                {
                    Language.English => "The weather is {0} **{1}** today",
                    Language.Russian => "Погода сегодня будет {0} **{1}**",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.WorldStateWeatherTomorrowDesc => language switch
                {
                    Language.English => "The weather will be {0} **{1}** tomorrow",
                    Language.Russian => "Погода завтра обещает быть {0} **{1}**",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.WorldStateSeasonDesc => language switch
                {
                    Language.English => "It's {0} **{1}** now, {2} **{3}** is coming {4}",
                    Language.Russian => "Сейчас {0} **{1}**, наступление {2} **{3}** {4}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentTimesDayQa => language switch
                {
                    Language.English => "What influences the time of day?",
                    Language.Russian => "На что влияет время суток?",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentWeatherQa => language switch
                {
                    Language.English => "What influences the weather?",
                    Language.Russian => "На что влияет погода?",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentSeasonQa => language switch
                {
                    Language.English => "What influences the season?",
                    Language.Russian => "На что влияет сезон?",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.WorldStateTimesDayDesc => language switch
                {
                    Language.English => "It's {0}, {1} **{2}**",
                    Language.Russian => "Сейчас {0}, {1} **{2}**",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.WorldInfoQaTimesDayAuthor => language switch
                {
                    Language.English => "Times of day",
                    Language.Russian => "Время суток",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.WorldInfoQaTimesDayDesc => language switch
                {
                    Language.English =>
                        "{0}, the time of day affects the types of fish you can catch while fishing. " +
                        "Some fish can only be caught at {1} night or vice versa only during the {2} day.",
                    Language.Russian =>
                        "{0}, время суток влияет на виды рыб, которые ты можешь поймать во время рыбалки. " +
                        "Некоторую рыбу можно поймать лишь {1} ночью или наоборот только {2} днем.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.WorldInfoQaWeatherAuthor => language switch
                {
                    Language.English => "Weather",
                    Language.Russian => "Погода",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.WorldInfoQaWeatherDesc => language switch
                {
                    Language.English =>
                        "{0}, the weather affects the types of fish you can catch while fishing. " +
                        "Some fish can only be caught in the {1} rain, or vice versa only in {2} sunny weather.",
                    Language.Russian =>
                        "{0}, погода влияет на виды рыб, которые ты можешь поймать во время рыбалки. " +
                        "Некоторую рыбу можно поймать лишь в {1} дождь или наоборот только при {2} солнечной погоде.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.WorldInfoQaSeasonAuthor => language switch
                {
                    Language.English => "Season",
                    Language.Russian => "Сезон",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.WorldInfoQaSeasonDesc => language switch
                {
                    Language.English =>
                        "{0}, the current season determines the assortment of seeds in the store, " +
                        "as well as the types of fish that you can catch while fishing. " +
                        "Some fish can only be caught in certain seasons.",
                    Language.Russian =>
                        "{0}, текущий сезон определяет ассортимент семян в магазине, " +
                        "а так же виды рыб, которые ты можешь поймать во время рыбалки. " +
                        "Некоторую рыбу можно поймать лишь в определенный сезон.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserTitlesAuthor => language switch
                {
                    Language.English => "Titles",
                    Language.Russian => "Титулы",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserTitlesDesc => language switch
                {
                    Language.English =>
                        "{0}, the titles you have earned are displayed here:\n\n" +
                        "{1} To change the current title, **select it** from the list below this message.",
                    Language.Russian =>
                        "{0}, тут отображаются полученные тобою титулы:\n\n" +
                        "{1} Для того чтобы изменить текущий титул, **выбери его** из списка под этим сообщением.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserTitleCurrentTitle => language switch
                {
                    Language.English => "Current title {0} {1} {2}",
                    Language.Russian => "Текущий титул {0} {1} {2}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.PaginatorFooter => language switch
                {
                    Language.English => "Page {0} of {1}",
                    Language.Russian => "Страница {0} из {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserTitlesTitleDesc => language switch
                {
                    Language.English => "Received {0}",
                    Language.Russian => "Получен {0}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentPaginatorBack => language switch
                {
                    Language.English => "Back",
                    Language.Russian => "Назад",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentPaginatorForward => language switch
                {
                    Language.English => "Forward",
                    Language.Russian => "Вперед",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserTitleUpdate => language switch
                {
                    Language.English => "Choose the title you want to set",
                    Language.Russian => "Выбери титул который хочешь установить",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserTitlesUpdateAuthor => language switch
                {
                    Language.English => "Update of the current title",
                    Language.Russian => "Обновление текущего титула",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserTitlesUpdateDesc => language switch
                {
                    Language.English => "{0}, you have successfully updated your title.",
                    Language.Russian => "{0}, ты успешно обновил свой титул.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserProfileUpdateAboutAuthor => language switch
                {
                    Language.English => "Change about in profile",
                    Language.Russian => "Изменение информации профиля",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserProfileUpdateAboutDesc => language switch
                {
                    Language.English => "{0}, your profile information has been successfully updated.",
                    Language.Russian => "{0}, информация в твоем профиле была успешно изменена.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUpdateAboutModalTitle => language switch
                {
                    Language.English => "Change profile about",
                    Language.Russian => "Изменение информации профиля",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUpdateAboutModalAbout => language switch
                {
                    Language.English => "Information to be displayed on your profile",
                    Language.Russian => "Информация, которая будет отображатся в твоем профиле",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUpdateAboutModalAboutLabel => language switch
                {
                    Language.English => "New about",
                    Language.Russian => "Информация",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserBannersAuthor => language switch
                {
                    Language.English => "Banners",
                    Language.Russian => "Баннеры",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserBannersDesc => language switch
                {
                    Language.English =>
                        "{0}, your banner collection is displayed here:\n\n" +
                        "{1} To set a banner on your profile, **select it** from the list below this message.",
                    Language.Russian =>
                        "{0}, тут отображается твоя коллекция баннеров:\n\n" +
                        "{1} Для того чтобы установить баннер в свой профиль, **выбери его** из списка под этим сообщением.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserBannersCurrentBanner => language switch
                {
                    Language.English => "Current banner {0} {1} {2} {3}",
                    Language.Russian => "Текущий баннер {0} {1} {2} {3}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserBannersBannerDesc => language switch
                {
                    Language.English => "[Click here to view]({0}). Ends {1}.",
                    Language.Russian => "[Нажми сюда чтобы посмотреть]({0}). Закончится {1}.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserBannerUpdate => language switch
                {
                    Language.English => "Choose the banner you want to set to your profile",
                    Language.Russian => "Выбери баннер который хочешь установить в профиль",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserBannersUpdateAuthor => language switch
                {
                    Language.English => "Update of the current banner",
                    Language.Russian => "Обновление текущего баннера",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserBannersUpdateDesc => language switch
                {
                    Language.English => "{0}, your banner has been successfully updated to {1} {2} «{3}».",
                    Language.Russian => "{0}, твой баннер успешно обновлен на {1} {2} «{3}».",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.PreconditionRequireLocationButYouFishing => language switch
                {
                    Language.English =>
                        "You need to wait until the fishing ends and you come back to the city to start a new task.",
                    Language.Russian =>
                        "сперва необходимо закончить с рыбалкой, или ты собрался бросить удочку и прыгнуть в воду?",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.PreconditionRequireLocationButYouFarmWatering => language switch
                {
                    Language.English =>
                        "you are watering the crops right now. " +
                        "Wait until you finish this task to start a new one.",
                    Language.Russian =>
                        "сперва необходимо закончить поливать семена, или ты собрался бросить лейку " +
                        "и оставить свой будущий урожай умирать без воды?",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.PreconditionRequireLocationButYouWorkOnContract => language switch
                {
                    Language.English =>
                        "under the terms of signed contract you must finish it before starting a new task. " +
                        "Wait until its end and you will be free to do whatever your heart desires",
                    Language.Russian =>
                        "по условиям рабочего контракта ты обязан сперва закончить над ним работу, " +
                        "а затем будешь волен делать что твоей душе угодно",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.PreconditionRequireLocationButYouAnotherLocation => language switch
                {
                    Language.English =>
                        "this action is only available in **{0}**, type </move:0> and select the appropriate location.",
                    Language.Russian =>
                        "это действие доступно лишь в **{0}**, напиши </move:0> и выбери соответствующую локацию.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentOwnerOnly => language switch
                {
                    Language.English => "this button is not for you!",
                    Language.Russian => "эта кнопка не для тебя!",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.FishingDesc => language switch
                {
                    Language.English =>
                        "{0}, **{1}** is full of people with fishing rods sitting here and there. " +
                        "You rent an old free boat and sail into the coastal zone for a catch. " +
                        "But even the most experienced fishermen cannot know in advance how well everything will turn out.",
                    Language.Russian =>
                        "{0}, **{1}** полна людей, сидящих то тут, то там с удочками. " +
                        "Ты арендуешь старую бесплатную лодку и плывешь в прибрежную зону за уловом. " +
                        "Но даже самые опытные рыбаки не могут знать заранее, насколько удачно все сложится.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.CubeDrops => language switch
                {
                    Language.English => "\n\n{0} {1} {2} dice rolled **{3}**!",
                    Language.Russian => "\n\nНа {0} {1} {2} кубиках выпало **{3}**!",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.FishingExpectedRewardTitle => language switch
                {
                    Language.English => "Expected reward",
                    Language.Russian => "Ожидаемая награда",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.FishingExpectedRewardDesc => language switch
                {
                    Language.English => "{0} exp and {1} random fish",
                    Language.Russian => "{0} ед. опыта и {1} случайная рыба",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.WillEndTitle => language switch
                {
                    Language.English => "Will end",
                    Language.Russian => "Закончится",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.CubeDropWaiting => language switch
                {
                    Language.English => "Waiting for the dice roll",
                    Language.Russian => "В ожидании броска кубиков",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentCubeDrop => language switch
                {
                    Language.English => "Roll the dice",
                    Language.Russian => "Бросить кубики",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.CubeDropPressButton => language switch
                {
                    Language.English =>
                        "\n\nPress the **Roll the dice** button to determine the duration of this process.",
                    Language.Russian =>
                        "\n\nНажми на кнопку **Бросить кубики** чтобы определить длительность этого процесса.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentCubeDropHowWorks => language switch
                {
                    Language.English => "Learn how cubes work",
                    Language.Russian => "Узнать как работают кубики",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.CompleteFishingSuccessDesc => language switch
                {
                    Language.English =>
                        "{0}, you come back with a smile on your face and proudly show the residents of the city the fish you have caught.\n" +
                        "There is something to be proud of, I understand, but there are still plenty of fish in the local waters, come back for a new catch as soon as possible!",
                    Language.Russian =>
                        "{0}, ты возвращаешься с улыбкой на лице и гордо демонстрируешь жителям города полученную добычу.\n" +
                        "Есть чем гордиться, понимаю, но рыбы в здешних водах еще полно, поскорее возвращайся за новым уловом!",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.CompleteFishingFailDesc => language switch
                {
                    Language.English =>
                        "{0}, this time you were out of luck, because when you returned you had nothing to show off to the people of the city.\n" +
                        "You almost caught {1} {2}, but the cunning fish managed to get off the hook. " +
                        "But do not worry, the fish in the local waters will not go anywhere, come back and try your luck again!",
                    Language.Russian =>
                        "{0}, в этот раз тебе не повезло, ведь вернувшись тебе совсем нечем похвастаться перед жителями города.\n" +
                        "Ты почти поймал {1} {2}, однако хитрая рыба смогла сорваться с крючка. " +
                        "Но не расстраивайся, рыба в здешних водах никуда не денется, возвращайся и попытай удачу еще раз!",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.CompleteFishingRewardTitle => language switch
                {
                    Language.English => "Reward received",
                    Language.Russian => "Полученная награда",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.CompleteFishingRewardSuccessDesc => language switch
                {
                    Language.English => "{0} {1} exp and {2} {3}",
                    Language.Russian => "{0} {1} ед. опыта и {2} {3}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.CompleteFishingRewardFailDesc => language switch
                {
                    Language.English => "{0} {1} exp",
                    Language.Russian => "{0} {1} ед. опыта",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserCollectionAuthor => language switch
                {
                    Language.English => "Collection",
                    Language.Russian => "Коллекция",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserCollectionDesc => language switch
                {
                    Language.English => "{0}, here your collection is displayed in the category **{1}**:",
                    Language.Russian => "{0}, тут отображается твоя коллекция в категории **{1}**:",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserCollectionCropString => language switch
                {
                    Language.English => "Spring harvest",
                    Language.Russian => "Весенний урожай",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserCollectionCropSummer => language switch
                {
                    Language.English => "Summer harvest",
                    Language.Russian => "Летний урожай",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserCollectionCropAutumn => language switch
                {
                    Language.English => "Autumn harvest",
                    Language.Russian => "Осенний урожай",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorAuthor => language switch
                {
                    Language.English => "Vendor",
                    Language.Russian => "Скупщик",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorDesc => language switch
                {
                    Language.English => "{0}, here are displayed the goods that the vendor is ready to buy:",
                    Language.Russian => "{0}, тут отображаются товары которые скупщик готов купить:",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorItemPrice => language switch
                {
                    Language.English => "Price: {0} {1} {2}",
                    Language.Russian => "Стоимость: {0} {1} {2}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentVendorSellFish => language switch
                {
                    Language.English => "Sell all fish",
                    Language.Russian => "Продать всю рыбу",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentVendorSellCrops => language switch
                {
                    Language.English => "Sell all crops",
                    Language.Russian => "Продать весь урожай",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorBulkSellDesc => language switch
                {
                    Language.English =>
                        "{0}, After closely observing the counting of goods, " +
                        "it is safe to say that the fence did not deceive you one coin. " +
                        "You didn't even notice how he made the list, but he handed it to you:",
                    Language.Russian =>
                        "{0}, после пристального наблюдения за пересчетом товаров, можно с уверенностью сказать, " +
                        "что скупщик не обманул тебя ни на монетку. Ты даже не заметил, как он составлял список, " +
                        "но он протянул его тебе:",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorBulkSellResultTitle => language switch
                {
                    Language.English => "Sales reporting",
                    Language.Russian => "Отчетность о продаже",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorBulkSellResultMoney => language switch
                {
                    Language.English => "\n\nTotal profit {0} {1} {2}",
                    Language.Russian => "\n\nИтоговая прибыль {0} {1} {2}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorBulkSellResultTooLong => language switch
                {
                    Language.English =>
                        "The sales reporting was so long that you decided to immediately look at the most important",
                    Language.Russian =>
                        "Отчестность была такой длинной, что ты решил сразу взглянуть на самое важное",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorBulkSellFishNothing => language switch
                {
                    Language.English => "you do not have a single fish that could be sold to a fence",
                    Language.Russian => "у тебя нет ни одной рыбы которую можно было бы продать скупщику.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorBulkSellCropsNothing => language switch
                {
                    Language.English => "you do not have a single crop that could be sold to a vendor",
                    Language.Russian => "у тебя нет ни одного урожая который можно было бы продать скупщику",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserProfileLocationTitle => language switch
                {
                    Language.English => "Currently",
                    Language.Russian => "Текущее занятие",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserProfileLocationTransit => language switch
                {
                    Language.English => "On the way from {0} **{1}** to {2} **{3}**, arrival {4}",
                    Language.Russian => "В пути из {0} **{1}** в {2} **{3}**, прибытие {4}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserProfileLocationFishingAndFarmWatering => language switch
                {
                    Language.English => "{0} **{1}**, will end {2}",
                    Language.Russian => "{0} **{1}**, завершение {2}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopBannerAuthor => language switch
                {
                    Language.English => "Shop banners",
                    Language.Russian => "Магазин баннеров",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopBannerDesc => language switch
                {
                    Language.English =>
                        "{0}, banners available for purchase are displayed here:\n\n" +
                        "{1} To purchase a banner, **select it** from the list below this message.\n" +
                        "{1} The banner is purchased for 30 days. Repeated purchase extends the duration of the banner.",
                    Language.Russian =>
                        "{0}, тут отображаются доступные для приобретения баннеры:\n\n" +
                        "{1} Для приобретения баннера, **выбери его** из списка под этим сообщением.\n" +
                        "{1} Баннер приобретается на 30 дней. Повторное приобретение продлевает длительность баннера.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopBannerCurrentCurrencyTitle => language switch
                {
                    Language.English => "Current currency for payment {0} {1} {2}",
                    Language.Russian => "Текущая валюта для оплаты {0} {1} {2}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopBannerBannerDesc => language switch
                {
                    Language.English => "[Click here to view]({0})\nPrice: {1} {2} {3} or {4} {5} {6}",
                    Language.Russian => "[Нажми сюда чтобы посмотреть]({0})\nСтоимость: {1} {2} {3} или {4} {5} {6}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentShopBannerSelectCurrencyToken => language switch
                {
                    Language.English => "Token payment",
                    Language.Russian => "Оплата токенами",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentShopBannerSelectCurrencyChip => language switch
                {
                    Language.English => "Chip payment",
                    Language.Russian => "Оплата чипами",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentShopBannerSelectBanner => language switch
                {
                    Language.English => "Choose the banner you want to buy",
                    Language.Russian => "Выбери баннер который хочешь приобрести",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopBannerBuyAuthor => language switch
                {
                    Language.English => "Purchasing a banner",
                    Language.Russian => "Приобретение баннера",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopBannerBuyDesc => language switch
                {
                    Language.English =>
                        "{0}, you have successfully purchased {1} {2} banner «{3}» for {4} {5} {6} for 30 days.\n\n" +
                        "{7} You can find the purchased banner in </banners:0>.",
                    Language.Russian =>
                        "{0}, ты успешно приобрел {1} {2} баннер «{3}» за {4} {5} {6} на 30 дней.\n\n" +
                        "{7} Найти приобретенный баннер можно в </banners:0>.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopBannerBuyNoCurrency => language switch
                {
                    Language.English => "you don't have enough {0} {1} to purchase {2} {3} banner {4}",
                    Language.Russian => "у тебя недостаточно {0} {1} для приобретения {2} {3} баннера «{4}».",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopSeedAuthor => language switch
                {
                    Language.English => "Shop seeds",
                    Language.Russian => "Магазин семян",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopSeedDesc => language switch
                {
                    Language.English =>
                        "{0}, here you can buy various seasonal seeds for growing crops:\n\n" +
                        "{1} To purchase seeds, **choose them** from the menu below this message.\n" +
                        "{1} This is a dynamic store with new products every season, don't miss it!",
                    Language.Russian =>
                        "{0}, тут можно приобрести различные сезонные семена для выращивания урожая:\n\n" +
                        "{1} Для прибретения семян, **выбери их** из меню под этим сообщением.\n" +
                        "{1} Это динамический магазин, товары которого обновляются каждый сезон, не пропускай!",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopSeedSeedDesc => language switch
                {
                    Language.English => "{0} will grow {1} {2} worth {3} {4} {5}",
                    Language.Russian => "{0} вырастет {1} {2} стоимостью {3} {4} {5}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopSeedSeedMultiply => language switch
                {
                    Language.English => "\n{0} *Grows several crops from one seed*",
                    Language.Russian => "\n{0} *Растет несколько шт. с одного семени*",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopSeedSeedReGrowth => language switch
                {
                    Language.English => "\n{0} *After the first harvest, it will re-growth {1}*",
                    Language.Russian => "\n{0} *После первого сбора будет давать повторный урожай {1}*",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopSeedSeedPrice => language switch
                {
                    Language.English => "{0} 5 {1} worth {2} {3} {4}",
                    Language.Russian => "{0} 5 {1} стоимостью {2} {3} {4}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentShopSeedBuy => language switch
                {
                    Language.English => "Choose the seeds you want to buy",
                    Language.Russian => "Выбери семена которые хочешь приобрести",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopSeedBuyAuthor => language switch
                {
                    Language.English => "Purchasing a seeds",
                    Language.Russian => "Приобретение семян",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopSeedBuyDesc => language switch
                {
                    Language.English => "{0}, you have successfully purchased {1} 5 {2} for {3} {4} {5}.",
                    Language.Russian => "{0}, ты успешно приобрел {1} 5 {2} за {3} {4} {5}.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopSeedBuyNoCurrency => language switch
                {
                    Language.English => "you don't have enough {0} {1} to buy {2} 5 {3}.",
                    Language.Russian => "у тебя недостаточно {0} {1} для приобретения {2} 5 {3}.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmAuthor => language switch
                {
                    Language.English => "Farm",
                    Language.Russian => "Ферма",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmDesc => language switch
                {
                    Language.English => "{0}, your {1} farm cells are displayed here:",
                    Language.Russian => "{0}, тут отображаются твои клетки {1} фермы:",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmFieldEmptyTitle => language switch
                {
                    Language.English => "Farm cell is empty",
                    Language.Russian => "Клетка фермы пустая",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmFieldEmptyDesc => language switch
                {
                    Language.English => "Plant seeds on it to start growing crops",
                    Language.Russian => "Посади на нее семена чтобы начать выращивать урожай",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmFieldPlantedTitle => language switch
                {
                    Language.English => "{0} {1}, will growth {2}",
                    Language.Russian => "{0} {1}, вырастет {2}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmFieldPlantedDesc => language switch
                {
                    Language.English => "Don't forget to water today",
                    Language.Russian => "Не забудь сегодня полить",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmFieldWateredTitle => language switch
                {
                    Language.English => "{0} {1}, will growth {2}",
                    Language.Russian => "{0} {1}, вырастет {2}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmFieldWateredDesc => language switch
                {
                    Language.English => "No need to water today",
                    Language.Russian => "Поливать сегодня уже не нужно",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmFieldCompletedTitle => language switch
                {
                    Language.English => "{0} {1}, can be harvested",
                    Language.Russian => "{0} {1}, можно собирать",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmFieldCompletedDesc => language switch
                {
                    Language.English => "Don't forget to plant something in the empty space",
                    Language.Russian => "Не забудь посадить что-то на освободившееся место",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmFieldCompletedReGrowthDesc => language switch
                {
                    Language.English => "After the first harvest, it will re-growth {0}",
                    Language.Russian => "После первого сбора будет давать повторный урожай {0}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmNeedToBuyDesc => language switch
                {
                    Language.English =>
                        "{0}, first you need to purchase {1} farm for {2} {3} {4}.\n\n" +
                        "{5} To purchase it, click the **Purchase farm** button below this message.",
                    Language.Russian =>
                        "{0}, сперва тебе необходимо приобрести {1} ферму за {2} {3} {4}.\n\n" +
                        "{5} Чтобы приобрести ее, нажми кнопку **Приобрести ферму** под этим сообщением.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserFarmPlant => language switch
                {
                    Language.English => "Plant seeds",
                    Language.Russian => "Посадить семена",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserFarmWater => language switch
                {
                    Language.English => "Water the seeds",
                    Language.Russian => "Полить семена",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserFarmCollect => language switch
                {
                    Language.English => "Harvest",
                    Language.Russian => "Собрать урожай",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserFarmDig => language switch
                {
                    Language.English => "Dig up the seeds",
                    Language.Russian => "Выкопать семена",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserFarmQaHarvesting => language switch
                {
                    Language.English => "How can I grow crops?",
                    Language.Russian => "Как мне выращивать урожай?",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserFarmQaUpgrading => language switch
                {
                    Language.English => "How can I expand the farm?",
                    Language.Russian => "Как мне расширить ферму?",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserFarmBuy => language switch
                {
                    Language.English => "Purchase farm",
                    Language.Russian => "Приобрести ферму",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmBuyAuthor => language switch
                {
                    Language.English => "Farm purchase",
                    Language.Russian => "Приобретение фермы",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmBuyDesc => language switch
                {
                    Language.English => "{0}, you have successfully purchased {1} farm for {2} {3} {4}.",
                    Language.Russian => "{0}, ты успешно приобрел {1} ферму за {2} {3} {4}.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmBuyNoCurrency => language switch
                {
                    Language.English => "you don't have enough {0} {1} to buy {2} farm.",
                    Language.Russian => "у тебя недостаточно {0} {1} для приобретения {2} фермы.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmQaHarvestingAuthor => language switch
                {
                    Language.English => "Growing a crops",
                    Language.Russian => "Выращивание урожая",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmQaHarvestingDesc => language switch
                {
                    Language.English =>
                        "{0}, to grow a crop, you need to follow a few simple steps:\n\n" +
                        "{1} First you need to purchase seeds in </shop-seeds:0>.\n\n" +
                        "{1} Then type </farm:0> and press the **Plant seeds** button.\n" +
                        "You'll go through a few quick steps that determine which seeds and {2} farm cells you want to plant.\n\n" +
                        "{1} Seeds need to be watered every day, for this type </farm:0> and press the **Water the seeds** button.\n\n" +
                        "{1} Once the seeds will grow, you can harvest them by typing </farm:0> and clicking on the **Harvest** button.\n\n" +
                        "{1} If you change your mind about growing seeds or want to replace them - type </farm:0> and press the **Dig up the seeds** button.\n" +
                        "You will need to select {2} farm cells from which seeds or crops will be removed.",
                    Language.Russian =>
                        "{0}, для выращивания урожая необходимо выполнить несколько простых шагов:\n\n" +
                        "{1} Для начала необходимо приобрести семена в </shop-seeds:0>.\n\n" +
                        "{1} Затем напиши </farm:0> и нажми на кнопку **Посадить семена**.\n" +
                        "Ты пройдешь несколько быстрых этапов, определяющих какие семена и на какие клетки {2} фермы ты хочешь посадить.\n\n" +
                        "{1} Семена необходимо поливать каждый день, для этого напиши </farm:0> и нажми на кнопку **Полить семена**.\n\n" +
                        "{1} После того как семена созреют, ты можешь собрать урожай, написав </farm:0> и нажав на кнопку **Собрать урожай**.\n\n" +
                        "{1} Если ты передумал выращивать семена или хочешь их заменить - напиши </farm:0> и нажми на кнопку **Выкопать семена**.\n" +
                        "Тебе необходимо будет выбрать клетки {2} фермы, с которых семена или урожай будет удален.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmQaUpgradingAuthor => language switch
                {
                    Language.English => "Farm expansion",
                    Language.Russian => "Расширение фермы",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmQaUpgradingDesc => language switch
                {
                    Language.English =>
                        "{0}, you can expand your {1} farm and increase the number of cells for growing crops.\n\n" +
                        "{2} For the {1} farm extension, click on the **Purchase farm extension** button.\n\n" +
                        "{3} The first expansion will cost you {4} {5} {6} and unlock **2 extra cells**.\n\n" +
                        "{7} The second expansion will cost you {4} {8} {9} and unlock **3 extra cells**.",
                    Language.Russian =>
                        "{0}, ты можешь расширить свою {1} ферму и увеличить количество ячеек для выращивания урожая.\n\n" +
                        "{2} Для расширения {1} фермы нажми на кнопку **Приобрести расширение фермы**.\n\n" +
                        "{3} Первое расширение обойдется тебе в {4} {5} {6} и откроет **2 дополнительных ячейки**.\n\n" +
                        "{7} Второе расширение обойдется тебе в {4} {8} {9} и откроет **3 дополнительных ячейки**.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentFarmUpgrade => language switch
                {
                    Language.English => "Purchase a farm expansion",
                    Language.Russian => "Приобрести расширение фермы",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmCollectAuthor => language switch
                {
                    Language.English => "Harvesting",
                    Language.Russian => "Сбор урожая",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmCollectDesc => language switch
                {
                    Language.English => "{0}, you have successfully harvested your {1} farm:",
                    Language.Russian => "{0}, ты успешно собрал урожай со своей {1} фермы:",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmCellTitle => language switch
                {
                    Language.English => "{0} {1} Farm cell `#{2}`",
                    Language.Russian => "{0} Ячейка {1} фермы `#{2}`",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmCellDesc => language switch
                {
                    Language.English => "You have successfully collected {0} {1} {2} and received {3} {4} exp",
                    Language.Russian => "Ты успешно собрал {0} {1} {2} и получил {3} {4} ед. опыта.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmCellReGrowth => language switch
                {
                    Language.English => "\n{0} A new crop will grow {1}.",
                    Language.Russian => "\n{0} Новый урожай вырастет {1}.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmCellEmpty => language switch
                {
                    Language.English => "\n{0} {1} Farm cell is now empty.",
                    Language.Russian => "\n{0} Ячейка {1} фермы теперь пустая.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmCollectNoCompletedCells => language switch
                {
                    Language.English => "Your {0} farm has no cells ready for harvest.",
                    Language.Russian => "на твоей {0} ферме нет ячеек с готовым для сбора урожаем.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmUpgradeAuthor => language switch
                {
                    Language.English => "Farm expansion",
                    Language.Russian => "Расширение фермы",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmUpgradeDesc => language switch
                {
                    Language.English => "{0}, you have successfully purchased {1} farm expansion for {2} {3} {4}.",
                    Language.Russian => "{0}, ты успешно приобрел {1} расширение фермы за {2} {3} {4}.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmUpgradeNoCurrency => language switch
                {
                    Language.English => "You don't have enough {0} {1} to purchase {2} farm expansion.",
                    Language.Russian => "у тебя недостаточно {0} {1} для приобретения {2} расширения фермы.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmWaterAuthor => language switch
                {
                    Language.English => "Farm watering",
                    Language.Russian => "Поливка фермы",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmWaterDesc => language switch
                {
                    Language.English => "{0}, you are going to water your {1} farm.",
                    Language.Russian => "{0}, ты отправляешься поливать свою {1} ферму.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmWaterNoPlatedCells => language switch
                {
                    Language.English => "there are no cells on your {0} farm that need watering.",
                    Language.Russian => "на твоей {0} ферме нет клеток которые нуждаются в поливке.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmWaterCompleted => language switch
                {
                    Language.English =>
                        "{0}, you have successfully watered the seeds on your {1} farm, now you can be sure that they will grow.\n\n" +
                        "{2} Gained {3} {4} exp",
                    Language.Russian =>
                        "{0}, ты успешно полил семена на своей {1} ферме, теперь можно быть уверенным в том, что они будут расти.\n\n" +
                        "{2} Получено {3} {4} ед. опыта",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmDigAuthor => language switch
                {
                    Language.English => "Digging",
                    Language.Russian => "Выкапывание",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmDigDesc => language switch
                {
                    Language.English =>
                        "{0}, first you need to **select cells** {1} of the farm from the list under this message, " +
                        "from which you want to dig seeds or crops:\n\n" +
                        "{2} Digging completely destroys planted seeds or grown crop.",
                    Language.Russian =>
                        "{0}, для начала необходимо **выбрать клетки** {1} фермы из списка под этим сообщением, " +
                        "с которых ты хочешь выкопать семена или урожай:\n\n" +
                        "{2} Выкапывание полностью уничтожает посаженные семена или выращенный урожай.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmDigCompleted => language switch
                {
                    Language.English =>
                        "{0}, you have successfully dug up seeds or crops from selected farm spaces {1}.",
                    Language.Russian => "{0}, ты успешно выкопал семена или урожай с выбранных клеток {1} фермы.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserFarmDigSelect => language switch
                {
                    Language.English => "Select the cells from which you want to dig up seeds or crops",
                    Language.Russian => "Выбери клетки с которых хочешь выкопать семена или урожай",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserFarmDigSelectLabel => language switch
                {
                    Language.English => "Farm cell #{0}",
                    Language.Russian => "Клетка фермы #{0}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserFarmDigSelectDesc => language switch
                {
                    Language.English => "Dig {0} from cell #{1}",
                    Language.Russian => "Выкопать {0} с клетки #{1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmPlantAuthor => language switch
                {
                    Language.English => "Planting seeds",
                    Language.Russian => "Посадка семян",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmPlantSelectSeedsDesc => language switch
                {
                    Language.English =>
                        "{0}, first you need to select the seeds that you want to plant on your {1} farm:\n\n" +
                        "{2} To plant seeds, **select them** from the list below this message.",
                    Language.Russian =>
                        "{0}, для начала необходимо выбрать семена которые ты хочешь посадить на свою {1} ферму:\n\n" +
                        "{2} Для посадки семян, **выбери их** из списка под этим сообщением.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmPlantSelectCellsDesc => language switch
                {
                    Language.English => "{0}, now you need to select the cells on which you want to plant {1} {2}:",
                    Language.Russian => "{0}, теперь необходимо выбрать клетки на которые ты хочешь посадить {1} {2}:",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmPlantSuccessDesc => language switch
                {
                    Language.English =>
                        "{0}, you have successfully planted {1} {2} on the selected cells of your {3} farm.",
                    Language.Russian => "{0}, ты успешно посадил {1} {2} на выбранные клетки своей {3} фермы.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserFarmPlantSelectSeed => language switch
                {
                    Language.English => "Choose the seeds you want to plant",
                    Language.Russian => "Выбери семена которые хочешь посадить",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserFarmPlantSelectCells => language switch
                {
                    Language.English => "Select the farm cells you want to plant seeds on",
                    Language.Russian => "Выбери клетки фермы на которые ты хочешь посадить семена",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentUserFarmPlantSelectCellsLabel => language switch
                {
                    Language.English => "Farm cell #{0}",
                    Language.Russian => "Клетка фермы #{0}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmPlantNoSeeds => language switch
                {
                    Language.English =>
                        "you have no seeds to plant on your {0} farm.\n\n" +
                        "{1} You can buy seeds at </shop-seeds:0>.",
                    Language.Russian =>
                        "у тебя нет семян которые можно было бы посадить на твою {0} ферму.\n\n" +
                        "{1} Приобрести семена можно в </shop-seeds:0>.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmPlantNoEmptyCells => language switch
                {
                    Language.English => "your {0} farm does not have any empty cells that you could plant {1} {2} on.",
                    Language.Russian => "на твоей {0} ферме нет пустых клеток, на которые ты мог бы посадить {1} {2}.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmPlantCellIsNotEmpty => language switch
                {
                    Language.English => "seeds have already been planted on the selected {0} farm cell.",
                    Language.Russian => "на выбранной клетке {0} фермы уже посажены семена.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserFarmPlantSelectSeedsSeedTitle => language switch
                {
                    Language.English => "{0} {1}, {2} in stock",
                    Language.Russian => "{0} {1}, в наличии {2} шт.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.LevelUpRewardAuthor => language switch
                {
                    Language.English => "Level up",
                    Language.Russian => "Повышение уровня",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.LevelUpRewardDesc => language switch
                {
                    Language.English => 
                        "{0}, after collecting enough {1} exp, you level up to {2} {3} " +
                        "and as a reward you get {4}",
                    Language.Russian =>
                        "{0}, набрав достаточное количество {1} ед. опыта, твой уровень повышается до {2} {3} " +
                        "и в качестве награды ты получаешь {4}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.LevelUpRewardTitle => language switch
                {
                    Language.English => 
                        "title {0} {1}.\n\n{2} You can find the received title in </titles:0>",
                    Language.Russian => 
                        "титул {0} {1}.\n\n{2} Найти полученный титул можно в </titles:0>",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.LevelUpRewardChips => language switch
                {
                    Language.English => 
                        "{0} {1} {2}.\n\n{3} You can find the received chips in </inventory:0>.",
                    Language.Russian => 
                        "{0} {1} {2}.\n\n{3} Найти полученные чипы можно в </inventory:0>.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.LevelUpRewardBanner => language switch
                {
                    Language.English => 
                        "{0} {1} banner «{2}».\n\n{3} You can find the received banner in </banners:0>.",
                    Language.Russian => 
                        "{0} {1} баннер «{2}».\n\n{3} Найти полученный баннер можно в </banners:0>.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.LevelUpRewardContainer => language switch
                {
                    Language.English => 
                        "{0} {1} {2}.\n\n{3} Received containers can be found and opened in </inventory:0>.",
                    Language.Russian =>
                        "{0} {1} {2}.\n\n{3} Найти и открыть полученные контейнеры можно в </inventory:0>.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.HowCubeDropWorksAuthor => language switch
                {
                    Language.English => "How cubes work",
                    Language.Russian => "Как работают кубики",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.HowCubeDropWorksDesc => language switch
                {
                    Language.English => 
                        "{0}, after pressing the button **Roll the dice** you roll your three dice and their sum" +
                        "is the result of a roll of dice and determines your success (the higher the value, the better).\n\n" +
                        "{1} By default your dice are {2}{3} D6, which means you can roll **3 to 18**.\n\n" +
                        "{1} Cube upgrades are in development.",
                    Language.Russian => 
                        "{0}, после нажатия на кнопку **Бросить кубики** ты бросаешь три своих кубика и их сумма " +
                        "становится результатом броска кубиков и определяет твой успех (чем выше значение - тем лучше).\n\n" +
                        "{1} По-умолчанию твои кубики это {2}{3} D6, что означает что ты можешь выбросить **от 3 до 18**.\n\n" +
                        "{1} Улучшение кубиков находится в разработке.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.AchievementAuthor => language switch
                {
                    Language.English => "Achievement earned",
                    Language.Russian => "Получено достижение",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.AchievementDesc => language switch
                {
                    Language.English => 
                        "{0}, you have completed {1} achievement **{2}** from category **{3}** and as a reward you will receive {4}\n\n" +
                        "{5} You can view your achievements in </achievements:0>.",
                    Language.Russian =>
                        "{0}, ты выполнил {1} достижение **{2}** из категории **{3}** и в качестве награды получаешь {4}\n\n" +
                        "{5} Посмотреть свои достижения можно в </achievements:0>.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.AchievementRewardChip => language switch
                {
                    Language.English => "{0} {1} {2}.\n\n{3} You can find the received chips in </inventory:0>.",
                    Language.Russian => "{0} {1} {2}.\n\n{3} Найти полученные чипы можно в </inventory:0>.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.AchievementRewardTitle => language switch
                {
                    Language.English => "title {0} {1}.\n\n{2} You can find the received title in </titles:0>.",
                    Language.Russian => "титул {0} {1}.\n\n{2} Найти полученный титул можно в </titles:0>.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserAchievementsAuthor => language switch
                {
                    Language.English => "Achievements",
                    Language.Russian => "Достижения",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserAchievementsDesc => language switch
                {
                    Language.English => "{0}, your achievements in the **{1}** category are displayed here:",
                    Language.Russian => "{0}, тут отображаются твои достижения в категории **{1}**:",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserAchievementsAchievementDescChip => language switch
                {
                    Language.English => "Reward: {0} {1} {2}, {3} achievement points",
                    Language.Russian => "Награда: {0} {1} {2}, {3} очков достижений",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserAchievementsAchievementDescTitle => language switch
                {
                    Language.English => "Reward: title {0} {1}, {2} achievement points",
                    Language.Russian => "Награда: титул {0} {1}, {2} очков достижений",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserAchievementsAchievementCompleted => language switch
                {
                    Language.English => "\n{0} Completed {1}",
                    Language.Russian => "\n{0} Выполнено {1}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.RatingTokensAuthor => language switch
                {
                    Language.English => "Rating by tokens",
                    Language.Russian => "Рейтинг токенов",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.RatingAchievementsAuthor => language switch
                {
                    Language.English => "Rating by achievements",
                    Language.Russian => "Рейтинг достижений",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.RatingXpAuthor => language switch
                {
                    Language.English => "Rating by expirience",
                    Language.Russian => "Рейтинг опыта",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.RatingEmpty => language switch
                {
                    Language.English => "There is no one in this rating yet, it's time for you to become the first!",
                    Language.Russian => "В этом рейтинге еще никого нет, самое время тебе стать первым!",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.RatingXpFieldDesc => language switch
                {
                    Language.English => "{0} `{1}` {2} {3} {4} {5} level, {6} {7} exp",
                    Language.Russian => "{0} `{1}` {2} {3} {4} {5} уровень, {6} {7} ед. опыта",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.RatingAchievementsFieldDesc => language switch
                {
                    Language.English => "{0} `{1}` {2} {3} {4} {5} achievement points",
                    Language.Russian => "{0} `{1}` {2} {3} {4} {5} очков достижений",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ContainerOpenAuthor => language switch
                {
                    Language.English => "Opening containers",
                    Language.Russian => "Открытие контейнеров",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ContainerOpenTokenDesc => language switch
                {
                    Language.English => "{0}, you open {1} {2} {3} and find {4} {5} {6} inside.",
                    Language.Russian => "{0}, ты открываешь {1} {2} {3} и обнаруживаешь внутри {4} {5} {6}.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ContainerOpenNoContainers => language switch
                {
                    Language.English => "you don't have any {0} {1} to open.",
                    Language.Russian => "у тебя нет в наличии ни одного {0} {1} чтобы открыть.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ComponentOpenExecutedChannel => language switch
                {
                    Language.English => "Open channel where you executed a command",
                    Language.Russian => "Открыть канал в котором ты использовал команду",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorSellDontHaveThatMuch => language switch
                {
                    Language.English => "you don't have as many {0} {1} as you want to sell.",
                    Language.Russian => "у тебя нет столько {0} {1} сколько ты хочешь продать.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorSellDesc => language switch
                {
                    Language.English => "{0}, you have successfully sold {1} {2} {3} for {4} {5} {6}.",
                    Language.Russian => "{0}, ты успешно продал {1} {2} {3} за {4} {5} {6}.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.SendCurrencyAuthor => language switch
                {
                    Language.English => "Sending currency",
                    Language.Russian => "Отправление валюты",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.SendCurrencyDesc => language switch
                {
                    Language.English => "{0}, you have successfully sent {1} {2} {3} to user {4}.",
                    Language.Russian => "{0}, ты успешно отправил {1} {2} {3} пользователю {4}.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.SendCurrencyNoCurrency => language switch
                {
                    Language.English => "you don't have as many {0} {1} as you want to send.",
                    Language.Russian => "у тебя нет столько {0} {1} сколько ты хочешь отправить.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.SendCurrencyNotifyDesc => language switch
                {
                    Language.English => "{0}, user {1} sent you {2} {3} {4}.",
                    Language.Russian => "{0}, пользователь {1} отправил тебе {2} {3} {4}.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(response), response, null)
            };
        }

        public static string Parse(this Response response, Language language)
        {
            return response.Localize(language);
        }

        public static string Parse(this Response response, Language language, params object[] replacements)
        {
            try
            {
                return string.Format(Parse(response, language), replacements);
            }
            catch (FormatException)
            {
                return "`An error occurred while displaying the response. Please show this on support server.`";
            }
        }
    }
}