using Zenject;
using UnityEngine;
using Helpers.Audio;
using Helpers.Services;
using Unity.Cinemachine;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class AppInit : MonoBehaviour
{
#region Fields
    
    [Header("Fmod Banks")]
    [SerializeField] BankLoader _bankLoaderMaster;
    [SerializeField] BankLoader _bankLoaderMasterStrings;
    
    [Header("Cameras")]
    [SerializeField] Camera _cameraMain;
    [SerializeField] Camera _cameraUI;
    [SerializeField] CinemachineStateDrivenCamera _stateDrivenCamera;
    
    [Header("Debugs")]
    [SerializeField] bool _enableDebug;
    [SerializeField] GameObject _graphyObj;

    StatesMachine _stateMachine;
    DiContainer _container;
    ICameraService _cameraService;
    ICinemachineService _cinemachineService;

#endregion

#region Monobehaviour

    [Inject]
    public void Construct(DiContainer container, 
                          ICameraService cameraService,
                          ICinemachineService cinemachineService)
    {
        _container = container;
        _cameraService = cameraService;
        _cinemachineService = cinemachineService;
    }

    async void Awake()
    {
#if !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
        
        _cameraService.RegisterMainCamera(_cameraMain);
        _cameraService.RegisterCamera(ConstCameras.CAMERA_UI, _cameraUI);
        SetDebugViews();
        SetCinemachineService();
        InitStateMachine();

        await LoadFMODBanks();

        _stateMachine.Enter<InitState>().GetAwaiter();
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
            _container.Instantiate<InitState>(),
            _container.Instantiate<MenuState>(),
            _container.Instantiate<GameState>()
        };
            
        _stateMachine = new StatesMachine(states);
    }

    void SetCinemachineService()
    {
        _cinemachineService.RegisterStateDrivenAnimator(_stateDrivenCamera.AnimatedTarget);
        
        var instructionIdx = 0;
        var stateDrivenInstructions = _stateDrivenCamera.Instructions;
        var states = (ECinemachineState[])System.Enum.GetValues(typeof(ECinemachineState));

        for (int i = 0; i < states.Length; i++)
        {
            var stateDrivenInsttruction = stateDrivenInstructions[i];
            var data = new StateDrivenCameraData(stateDrivenInsttruction.Camera, stateDrivenInsttruction.FullHash);
            _cinemachineService.AddStateDrivenCameraData(states[i].ToString(), data);
        }
    }

    async UniTask LoadFMODBanks()
    {
        await _bankLoaderMasterStrings.Init();
        await _bankLoaderMaster.Init();
    }

#endregion
}
}