using Cysharp.Threading.Tasks;

namespace Source
{
public interface ILocaleService
{
    int LocalesCount { get; }

    /// <summary>
    /// Waits until Unity Localization is initialized, call before first <see cref="SetLocale"/>.
    /// </summary>
    UniTask Init();

    void SetLocale(int localeIdx);
}
}
