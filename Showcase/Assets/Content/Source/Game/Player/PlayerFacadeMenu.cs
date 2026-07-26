using Zenject;
using UnityEngine;
using FMOD.Studio;
using Cysharp.Threading.Tasks;

namespace Source.Player
{
public class PlayerFacadeMenu : MonoBehaviour, IPlayerFacade
{
    [SerializeField] PlayerEmergence _playerEmergence;
    [SerializeField] PlayerAppearenceBase _playerAppearence;
    
    [Space]
    [SerializeField] Rigidbody2D _rb;
    
    [Inject] IAudioService _audioService;

    public void Init()
    {
        _audioService.FlyInstance.start();
        
        _playerAppearence.Init();
        _playerEmergence.Init(_rb);
    }

    public void Deinit() => _audioService.FlyInstance.stop(STOP_MODE.IMMEDIATE);

    public async UniTask ShowPlayer() => await _playerEmergence.ShowPlayer(_rb);
}
}