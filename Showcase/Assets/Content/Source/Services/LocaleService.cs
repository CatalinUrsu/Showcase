using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Localization.Settings;

namespace Source.Services
{
public class LocaleService : ILocaleService
{
    public int LocalesCount => LocalizationSettings.AvailableLocales.Locales.Count;

    public async UniTask Init() => await LocalizationSettings.InitializationOperation.Task;

    public void SetLocale(int localeIdx)
    {
        var locales = LocalizationSettings.AvailableLocales.Locales;
        LocalizationSettings.SelectedLocale = locales[Mathf.Clamp(localeIdx, 0, locales.Count - 1)];
    }
}
}
