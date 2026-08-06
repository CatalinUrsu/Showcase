using Cysharp.Threading.Tasks;
using FMOD.Studio;
using UnityEngine;
using Zenject;

namespace Source.Game.Player
{
public class PlayerFacade : MonoBehaviour, IPlayerFacade
{
    [SerializeField] protected Rigidbody2D _rb;
    
    [Space]
    [SerializeField] protected PlayerEmergence _playerEmergence;
    [SerializeField] protected PlayerAppearanceBase _playerAppearance;
    
    [Inject] protected IAudioService _audioService;

    public virtual void Init()
    {
        _audioService.FlyInstance.start();
        
        _playerAppearance.Init();
        _playerEmergence.Init(_rb);
    }

    public virtual void Deinit() => _audioService.FlyInstance.stop(STOP_MODE.IMMEDIATE);

    public virtual void ToggleControl(bool enable) { }
    
    public virtual async UniTask ShowPlayer() => await _playerEmergence.ShowPlayer(_rb);
}
}