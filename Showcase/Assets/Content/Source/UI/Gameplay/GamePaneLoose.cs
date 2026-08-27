using R3;
using Zenject;
using UnityEngine;
using IdleNumbers;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Source.UI.Gameplay
{
public class GamePaneLoose : GamePanel
{
#region Fields

    [Space]
    [SerializeField] LocalizeStringEvent _txtCollectedCoins;
    [SerializeField] LocalizeStringEvent _txtTotal;
    
    [Space]
    [SerializeField] ButtonBase _buttonRestart;
    [SerializeField] ButtonBase _buttonHome;

    readonly StringVariable _coinsStringVar = new();
    readonly StringVariable _totalStringVar = new();
    
    [Inject] IGameRunModelController _gameRunModelController;
    [Inject] IProgressModelController _progressModelController;

#endregion

#region Public methods

    public override void Init()
    {
        base.Init();

        _buttonHome.Init();
        _buttonRestart.Init();
        
        _txtCollectedCoins.StringReference.Arguments = new[] { _coinsStringVar };
        _txtTotal.StringReference.Arguments = new[] { _totalStringVar };

        _buttonHome.Btn.onClick.AddListener(() => _gameRunModelController.OnClickGoHome_raise());
        _buttonRestart.Btn.onClick.AddListener(() => _gameRunModelController.OnClickRestartRun_raise());
        
        _gameRunModelController.IModel.CollectedCoinsRef
                               .Subscribe(UpdateCollectedCoinsValue)
                               .AddTo(this);
        
        _progressModelController.IModel.CoinsRef
                               .Subscribe(UpdateTotalCoinsValue)
                               .AddTo(this);
    }

    void UpdateCollectedCoinsValue(IdleNumber value)
    {
        _coinsStringVar.Value = value.AsString();
        _txtCollectedCoins.RefreshString();
    }

    void UpdateTotalCoinsValue(IdleNumber value)
    {
        _totalStringVar.Value = value.AsString();
        _txtTotal.RefreshString();
    }

#endregion
}
}