using Zenject;
using UnityEngine;

namespace Source.Boot
{
/// <summary>
/// Lives under ProjectContext (AppInit's scene is unloaded), flushes save when app goes to background or quits.
/// </summary>
public class AppLifecycleListener : MonoBehaviour
{
    ISaveService _saveService;

    [Inject]
    public void Construct(ISaveService saveService) => _saveService = saveService;

    // Mobile OS kills paused process without OnApplicationQuit, pause is the last reliable moment
    void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
            _saveService.Flush();
    }

    void OnApplicationQuit() => _saveService.Flush();
}
}
