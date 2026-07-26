using Zenject;
using Helpers;
using System.Linq;
using UnityEngine;
using Helpers.Audio;
using Helpers.Services;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace Source
{
public class AppInit : MonoBehaviour
{
#region Fields

    [SerializeField] InitValuesForSaves _initValuesForSaves;
    [SerializeField] GameObject _singletonParent;
    
    [Header("Fmod Banks")]
    [SerializeField] AssetReference _masterAssetRef;
    [SerializeField] AssetReference _masterStringAssetRef;
    
    [Header("Cameras")]
    [SerializeField] Camera _cameraMain;
    [SerializeField] Camera _cameraUI;
    
    [Header("Debugs")]
    [SerializeField] bool _enableDebug;
    [SerializeField] GameObject _graphyObj;

    StatesMachine _stateMachine;
    ISceneLoaderService _sceneLoaderService;
    IProgressTrackingService _progressTrackingService;
    ICameraService _cameraService;
    IAudioService _audioService;
    ISplashScreen _splashScreen;

#endregion

#region Monobehaviour

    [Inject]
    public void Construct(ISceneLoaderService sceneLoaderService, ISplashScreen splashScreen,
                          IProgressTrackingService progressTrackingService, ICameraService cameraService,
                          IAudioService audioService)
    {
        _audioService = audioService;
        _sceneLoaderService = sceneLoaderService;
        _splashScreen = splashScreen;
        _progressTrackingService = progressTrackingService;
        _cameraService = cameraService;
    }

    async void Awake()
    {
#if !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif

        _initValuesForSaves.LoadSavedItems();
        _cameraService.RegisterMainCamera(_cameraMain);
        _cameraService.RegisterCamera(ConstCameras.CAMERA_UI, _cameraUI);
        SetDebugViews();

        await UniTask.WhenAll(LoadFMODBanks(),
                              SetSingletons());

        SetStateMachine();
    }

#endregion

#region Private methods
    
    void SetDebugViews()
    {
        if (!_enableDebug) return;

        Instantiate(_graphyObj);
    }

    void SetStateMachine()
    {
        var states = new IState[]
        {
            new StateInit(_sceneLoaderService, _splashScreen, _audioService),
            new StateMenu(_sceneLoaderService, _splashScreen, _progressTrackingService, _audioService),
            new StateGameplay(_sceneLoaderService, _splashScreen, _progressTrackingService, _audioService)
        };

        _stateMachine = new StatesMachine(states);

        _stateMachine.Enter<StateInit>().GetAwaiter();
    }

    async UniTask LoadFMODBanks()
    {
        await _masterAssetRef.LoadBank();
        await _masterStringAssetRef.LoadBank();
    }

    async UniTask SetSingletons()
    {
        var singletons = new List<Singleton<Component>>();
        foreach (Transform child in _singletonParent.transform)
        {
            if (child.TryGetComponent<Singleton<Component>>(out var singleton))
                singletons.Add(singleton);
        }

        await UniTask.WaitUntil(() => singletons.All(singleton => singleton.IsSet));
    }

#endregion
}
}