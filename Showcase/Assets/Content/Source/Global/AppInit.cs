using Zenject;
using Helpers;
using System.Linq;
using UnityEngine;
using Source.Audio;
using Helpers.Audio;
using Helpers.Services;
using Source.StateMachine;
using Helpers.StateMachine;
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
    IServiceSceneLoader _serviceSceneLoader;
    IServiceSplashScreen _serviceSplashScreen;
    IServiceProgressTracking _serviceLoadingProgress;
    IServiceCamera _serviceCamera;
    IAudioService _audioService;

#endregion

#region Monobehaviour

    [Inject]
    public void Construct(IServiceSceneLoader serviceSceneLoader, IServiceSplashScreen serviceSplashScreen,
                          IServiceProgressTracking serviceLoadingProgress, IServiceCamera serviceCamera,
                          IAudioService audioService)
    {
        _audioService = audioService;
        _serviceSceneLoader = serviceSceneLoader;
        _serviceSplashScreen = serviceSplashScreen;
        _serviceLoadingProgress = serviceLoadingProgress;
        _serviceCamera = serviceCamera;
    }

    async void Awake()
    {
#if !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif

        _initValuesForSaves.LoadSavedItems();
        _serviceCamera.RegisterMainCamera(_cameraMain);
        _serviceCamera.RegisterCamera(ConstCameras.CAMERA_UI, _cameraUI);
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
            new StateInit(_serviceSceneLoader, _serviceSplashScreen, _audioService),
            new StateMenu(_serviceSceneLoader, _serviceSplashScreen, _serviceLoadingProgress, _audioService),
            new StateGameplay(_serviceSceneLoader, _serviceSplashScreen, _serviceLoadingProgress, _audioService)
        };

        _stateMachine = new StatesMachine(states);

        _stateMachine.Enter<StateInit>().GetAwaiter();
    }

    async UniTask LoadFMODBanks()
    {
        (await _masterAssetRef.LoadTextAsset()).LoadBank();
        (await _masterStringAssetRef.LoadTextAsset()).LoadBank();
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