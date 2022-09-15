using Disfarm.Data.Enums;

namespace Disfarm.Services.Game.Localization
{
    public interface ILocalizationService
    {
        string Localize(LocalizationCategory category, string keyword, Language language, uint amount = 1);
    }
}
