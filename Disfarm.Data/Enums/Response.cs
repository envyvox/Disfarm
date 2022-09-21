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
        FishingWillEndTitle,
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
        VendorSellDesc,
        VendorSellResultTitle,
        VendorSellResultMoney,
        VendorSellResultTooLong,
        UserProfileLocationTitle,
        UserProfileLocationTransit,
        UserProfileLocationFishingAndFarmWatering,
        ShopBannerAuthor,
        ShopBannerDesc,
        ShopBannerCurrentCurrencyTitle,
        ShopBannerBannerDesc,
        ShopBannerBuyAuthor,
        ShopBannerBuyDesc,


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

        // exceptions
        SomethingWentWrongTitle,
        SomethingWentWrongDesc,
        SettingsLanguageAlready,
        PreconditionRequireLocationButYouFishing,
        PreconditionRequireLocationButYouFarmWatering,
        PreconditionRequireLocationButYouWorkOnContract,
        PreconditionRequireLocationButYouAnotherLocation,
        ComponentOwnerOnly,
        VendorSellFishNothing,
        VendorSellCropsNothing,
        ShopBannerBuyNoCurrency,
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
                        "{0}, something unusual happened and I already reported it to the development team. I apologize for my dummy creators, they will definitely improve.",
                    Language.Russian =>
                        "{0}, произошло что-то необычное и я уже сообщила об этом команде разработки. Приношу извинения за моих глупых создателей, они обязательно исправятся.",
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
                        "You have too many fish, type {0} `/inventory` and select a **fish category** to view it",
                    Language.Russian =>
                        "У тебя слишком много рыбы, напиши {0} `/инвентарь` и выбери **категорию рыбы**, чтобы посмотреть ее",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventoryTooMuchSeeds => language switch
                {
                    Language.English =>
                        "You have too many seeds, type {0} `/inventory` and select a **seeds category** to view it",
                    Language.Russian =>
                        "У тебя слишком много семян, напиши {0} `/инвентарь` и выбери **категорию семян**, чтобы посмотреть их",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserInventoryTooMuchCrops => language switch
                {
                    Language.English =>
                        "You have too many crops, type {0} `/inventory` and select a **crops category** to view it",
                    Language.Russian =>
                        "У тебя слишком много урожая, напиши {0} `/инвентарь` и выбери **категорию урожая**, чтобы посмотреть его",
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
                    Language.English => "The weather will be {0} **{1}** today",
                    Language.Russian => "Погода сегодня будет {0} **{1}**",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.WorldStateWeatherTomorrowDesc => language switch
                {
                    Language.English => "The weather promises to be {0} **{1}** tomorrow",
                    Language.Russian => "Погода завтра обещает быть {0} **{1}**",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.WorldStateSeasonDesc => language switch
                {
                    Language.English => "It's {0} **{1}** now, {2} **{3}** will be {4}",
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
                        "{0}, the time of day affects the types of fish you can catch while fishing. Some fish can only be caught at {1} night or vice versa only during the {2} day.",
                    Language.Russian =>
                        "{0}, время суток влияет на виды рыб, которые ты можешь поймать во время рыбалки. Некоторую рыбу можно поймать лишь {1} ночью или наоборот только {2} днем.",
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
                        "{0}, the weather affects the types of fish you can catch while fishing. Some fish can only be caught in the {1} rain, or vice versa only in {2} sunny weather.",
                    Language.Russian =>
                        "{0}, погода влияет на виды рыб, которые ты можешь поймать во время рыбалки. Некоторую рыбу можно поймать лишь в {1} дождь или наоборот олько при {2} солнечной погоде.",
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
                        "{0}, the current season determines the assortment of seeds in the store, as well as the types of fish that you can catch while fishing. Some fish can only be caught in certain seasons.",
                    Language.Russian =>
                        "{0}, текущий сезон определяет ассортимент семян в магазине, а так же виды рыб, которые ты можешь поймать во время рыбалки. Некоторую рыбу можно поймать лишь в определенный сезон.",
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
                        "{0}, the titles you have earned are displayed here:\n\n{1} To change the current title, **select it** from the list below this post.",
                    Language.Russian =>
                        "{0}, тут отображаются полученные тобою титулы:\n\n{1} Для того чтобы изменить текущий титул, **выбери его** из списка под этим сообщением.",
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
                        "{0}, your banner collection is displayed here:\n\n{1} To set a banner on your profile, **select it** from the list below this message.",
                    Language.Russian =>
                        "{0}, тут отображается твоя коллекция баннеров:\n\n{1} Для того чтобы установить баннер в свой профиль, **выбери его** из списка под этим сообщением.",
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
                        "first you need to finish fishing, or are you going to throw a fishing rod and jump into the water?",
                    Language.Russian =>
                        "сперва необходимо закончить с рыбалкой, или ты собрался бросить удочку и прыгнуть в воду?",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.PreconditionRequireLocationButYouFarmWatering => language switch
                {
                    Language.English =>
                        "you need to finish watering the seeds first, or you are going to throw away the watering can and leave your future crop to die without water",
                    Language.Russian =>
                        "сперва необходимо закончить поливать семена, или ты собрался бросить лейку и оставить свой будущий урожай умирать без воды?",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.PreconditionRequireLocationButYouWorkOnContract => language switch
                {
                    Language.English =>
                        "under the terms of the work contract, you must first finish work on it, and then you will be free to do whatever your heart desires",
                    Language.Russian =>
                        "по условиям рабочего контракта ты обязан сперва закончить над ним работу, а затем будешь волен делать что твоей душе угодно",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.PreconditionRequireLocationButYouAnotherLocation => language switch
                {
                    Language.English =>
                        "this action is only available in **{0}**, type {1} `/move` and select the appropriate location.",
                    Language.Russian =>
                        "это действие доступно лишь в **{0}**, напиши {1} `/move` и выбери соответствующую локацию.",
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
                        "{0}, **{1}** is full of people who want to catch a cool catch and now you are one of them. In the hope that the goddess of fortune will send you a harder catch, you go fishing, but even the most experienced fishermen cannot know in advance how well everything will turn out.",
                    Language.Russian =>
                        "{0}, **{1}** полна желающих поймать крутой улов и теперь ты один из них. В надежде что богиня фортуны пошлет тебе улов потяжелее ты отправляешься на рыбалку, но даже самые опытные рыбаки не могут знать заранее насколько удачно все пройдет.",
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
                Response.FishingWillEndTitle => language switch
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
                        "\n\nPress the button **Roll the dice** to determine the duration of this process.",
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
                        "{0}, you come back with a smile on your face and proudly show the residents of the city the fish you have received.\nThere is something to be proud of, I understand, but there are still plenty of fish in the local waters, come back for a new catch as soon as possible!",
                    Language.Russian =>
                        "{0}, ты возвращаешься с улыбкой на лице и гордо демонстрируешь жителям города полученную рыбу.\nЕсть чем гордиться, понимаю, но рыбы в здешних водах еще полно, возвращайся за новым уловом поскорее!",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.CompleteFishingFailDesc => language switch
                {
                    Language.English =>
                        "{0}, this time you were out of luck, because when you returned you had nothing to show off to the people of the city.\nYou almost caught {1} {2}, but the cunning fish managed to get off the hook. But do not worry, the fish in the local waters will not go anywhere, come back and try your luck again!",
                    Language.Russian =>
                        "{0}, в этот раз тебе не повезло, ведь вернувшись тебе совсем нечем похвастаться перед жителями города.\nТы почти поймал {1} {2}, однако хитрая рыба смогла сорваться с крючка. Но не расстраивайся, рыба в здешних водах никуда не денется, возвращайся и попытай удачу еще раз!",
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
                Response.VendorSellDesc => language switch
                {
                    Language.English =>
                        "{0}, after a fairly quick recalculation of goods with a vendor, this is what happened in the end:",
                    Language.Russian =>
                        "{0}, после достаточно быстрого пересчета товаров со скупщиком, вот что получилось в итоге:",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorSellResultTitle => language switch
                {
                    Language.English => "Sales reporting",
                    Language.Russian => "Отчетность о продаже",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorSellResultMoney => language switch
                {
                    Language.English => "\n\nTotal profit {0} {1} {2}",
                    Language.Russian => "\n\nИтоговая прибыль {0} {1} {2}",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorSellResultTooLong => language switch
                {
                    Language.English =>
                        "The sales reporting was so long that you decided to immediately look at the most important",
                    Language.Russian =>
                        "Отчестность была такой длинной, что ты решил сразу взглянуть на самое важное",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorSellFishNothing => language switch
                {
                    Language.English => "you do not have a single fish that could be sold to a fence",
                    Language.Russian => "у тебя нет ни одной рыбы которую можно было бы продать скупщику.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.VendorSellCropsNothing => language switch
                {
                    Language.English => "you do not have a single crop that could be sold to a vendor",
                    Language.Russian => "у тебя нет ни одного урожая который можно было бы продать скупщику",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.UserProfileLocationTitle => language switch
                {
                    Language.English => "Current location",
                    Language.Russian => "Текущая локация",
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
                        "{7} You can find the purchased banner in {8} `/banners`.",
                    Language.Russian =>
                        "{0}, ты успешно приобрел {1} {2} баннер «{3}» за {4} {5} {6} на 30 дней.\n\n" +
                        "{7} Найти приобретенный баннер можно в {8} `/banners`.",
                    _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
                },
                Response.ShopBannerBuyNoCurrency => language switch
                {
                    Language.English => "you don't have enough {0} {1} to purchase {2} {3} banner {4}",
                    Language.Russian => "у тебя недостаточно {0} {1} для приобретения {2} {3} баннера «{4}».",
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