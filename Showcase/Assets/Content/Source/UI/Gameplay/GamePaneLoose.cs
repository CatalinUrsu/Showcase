using TMPro;
using Zenject;
using UnityEngine;
using IdleNumbers;
using Source.Gameplay;
using Cysharp.Threading.Tasks;
using Source.Data;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Source.UI.Gameplay
{
public class GamePaneLoose : GamePanel
{
#region Fields

    [SerializeField] LocalizeStringEvent _localizedStringCollected;
    [SerializeField] TextMeshProUGUI _txtTotal;
    [SerializeField] ButtonBase _buttonRestart;
    [SerializeField] ButtonBase _buttonHome;

    [Inject] GameRunModel _gameRunModel;

#endregion

#region Public methods

    public override void Init()
    {
        base.Init();

        _buttonHome.Init();
        _buttonRestart.Init();
        _buttonHome.onClick.AddListener(() => GameplayMediator.SetGameState(EGameplayState.Leave));
        _buttonRestart.onClick.AddListener(() => GameplayMediator.SetGameState(EGameplayState.NewGame));
    }

    public override async UniTask Show()
    {
        (_localizedStringCollected.StringReference["0"] as StringVariable)!.Value = $"{_gameRunModel.CollectedCoins.Value.AsString()} {ConstSpriteAssets.SPRITE_TEXT_COIN}";
        _txtTotal.SetText($"{SessionService.Current.Progress.Coins.Value.AsString()}  {ConstSpriteAssets.SPRITE_TEXT_COIN}");
        
        await base.Show();
    }

#endregion
}
}