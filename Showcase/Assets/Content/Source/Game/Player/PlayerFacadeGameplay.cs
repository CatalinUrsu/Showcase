using R3; 
using Zenject;
using UnityEngine;
using Source.Gameplay;
using Cysharp.Threading.Tasks;

namespace Source.Game.Player
{
[RequireComponent(typeof(PlayerEffects), typeof(PlayerMovement), typeof(PlayerWeapons))]
[RequireComponent(typeof(PlayerParallaxEffect))]
public class PlayerFacadeGameplay : PlayerFacade
{
#region Fields

    [SerializeField] PlayerEffects _playerEffects;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] PlayerWeapons _playerWeapons;
    [SerializeField] PlayerParallaxEffect _playerParallaxEffect;
    
    IGameRunModelController  _runModelController;

#endregion
    
#region Monobeh

    protected override void OnValidate()
    {
        base.OnValidate();
        
        _playerEffects ??= GetComponent<PlayerEffects>();
        _playerMovement ??= GetComponent<PlayerMovement>();
        _playerWeapons ??= GetComponent<PlayerWeapons>();
        _playerParallaxEffect ??= GetComponent<PlayerParallaxEffect>();
    }

    [Inject]
    public void Construct(IGameRunModelController runModelController) => _runModelController = runModelController;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out EnemyHitTrigger enemyCollider)) return;
        
        DestroyPlayer();
        _runModelController.KillPlayer();
        enemyCollider.OnHitPlayer_raise();
    }

#endregion
    
#region Public methods

    public override void Init()
    {
        base.Init();
        _runModelController.IModel.IsPlayingNewLvlAnimRef.Subscribe(PlayNewLvlEffect).AddTo(this);

        _playerEffects.Init();
        _playerMovement.Init(_rb);
        _playerWeapons.Init();
    }

    public override void Deinit()
    {
        base.Deinit();
        
        _playerEffects.Deinit();
        _playerMovement.Deinit();
        _playerWeapons.Deinit();
        _playerParallaxEffect.Deinit();
    }

    public override async UniTask ShowPlayer()
    {
        _playerAppearance.ToggleAppearance(true);
        _playerEffects.EnableShield().Forget();
        _playerMovement.PlayMovementFx();
        await _playerEmergence.ShowPlayer(_rb);

        _playerParallaxEffect.Enable();
    }

    public override void ToggleControl(bool enable)
    {
        _playerMovement.ToggleControl(enable);
        _playerWeapons.ToggleShooting(enable);
    }

#endregion

#region Private methods

    void DestroyPlayer()
    {
        _playerAppearance.ToggleAppearance(false);
        _playerEffects.PlayDeathEffects();
        _playerMovement.StopMovementFx();
        _playerParallaxEffect.DisableParallax();
        
        ToggleControl(false);
    }

    void PlayNewLvlEffect(bool isNewLvl)
    {
        if (isNewLvl)
            _playerEffects.EnableShield().Forget();
    }
    
#endregion
}
}