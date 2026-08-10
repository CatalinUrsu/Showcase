using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Source.Game.Player
{
[RequireComponent(typeof(Rigidbody), typeof(PlayerAppearance), typeof(PlayerEmergence))]
public class PlayerFacade : MonoBehaviour, IPlayerFacade
{
    [SerializeField] protected Rigidbody2D _rb;

    [Space]
    [SerializeField] protected PlayerEmergence _playerEmergence;
    [SerializeField] protected PlayerAppearance _playerAppearance;

    protected virtual void OnValidate()
    {
        _rb ??= GetComponent<Rigidbody2D>();
        _playerEmergence ??= GetComponent<PlayerEmergence>();
        _playerAppearance ??= GetComponent<PlayerAppearance>();
    }

    public virtual void Init()
    {
        _playerAppearance.Init();
        _playerEmergence.Init(_rb);
    }

    public virtual void Deinit() => _playerEmergence.Deinit();

    public virtual async UniTask ShowPlayer() => await _playerEmergence.ShowPlayer(_rb);
    
    public virtual void ToggleControl(bool enable) { }
}
}