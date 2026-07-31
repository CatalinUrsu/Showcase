using Zenject;
using UnityEngine;
using Helpers.Audio;
using Helpers.Services;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Source.Boot
{
public class AppInit : MonoBehaviour
{
#region Fields
    
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
    DiContainer _container;
    ICameraService _cameraService;

#endregion

#region Monobehaviour

    [Inject]
    public void Construct(DiContainer container, ICameraService cameraService)
    {
        _container = container;
        _cameraService = cameraService;
    }

    async void Awake()
    {
#if !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
        
        _cameraService.RegisterMainCamera(_cameraMain);
        _cameraService.RegisterCamera(ConstCameras.CAMERA_UI, _cameraUI);
        SetDebugViews();
        InitStateMachine();

        await LoadFMODBanks();

        _stateMachine.Enter<StateInit>().GetAwaiter();
    }

#endregion

#region Private methods
    
    void SetDebugViews()
    {
        if (!_enableDebug) return;

        Instantiate(_graphyObj);
    }
    
    void InitStateMachine()
    {
        var states = new IState[]
        {
            _container.Instantiate<StateInit>(),
            _container.Instantiate<StateMenu>(),
            _container.Instantiate<StateGameplay>()
        };
            
        _stateMachine = new StatesMachine(states);
    }

    async UniTask LoadFMODBanks()
    {
        await _masterAssetRef.LoadBank();
        await _masterStringAssetRef.LoadBank();
    }

#endregion
}
}