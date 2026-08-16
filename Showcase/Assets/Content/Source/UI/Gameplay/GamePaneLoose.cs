using R3;
using TMPro;
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
    [SerializeField] TextMeshProUGUI _txtTotal;
    
    [Space]
    [SerializeField] ButtonBase _buttonRestart;
    [SerializeField] ButtonBase _buttonHome;

    [Inject] IGameRunModelController _gameRunModelController;
    readonly StringVariable _coinsStringVar = new();

#endregion

#region Public methods

    public override void Init()
    {
        base.Init();

        _buttonHome.Init();
        _buttonRestart.Init();
        
        _txtCollectedCoins.StringReference.Arguments = new[] { _coinsStringVar };

        _buttonHome.Btn.onClick.AddListener(() => _gameRunModelController.OnClickGoHome_raise());
        _buttonRestart.Btn.onClick.AddListener(() => _gameRunModelController.OnClickReturnToGame_raise());
        _gameRunModelController.IModel.CollectedCoinsRef
                               .Skip(1)
                               .Subscribe(UpdateCollectedCoinsValue)
                               .AddTo(this);
    }

    void UpdateCollectedCoinsValue(IdleNumber value) => _coinsStringVar.Value = value.AsString();

#endregion
}
}