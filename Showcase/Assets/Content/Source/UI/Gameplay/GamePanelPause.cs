using UnityEngine;
using Source.Gameplay;

namespace Source.UI.Gameplay
{
public class GamePanelPause : GamePanel
{
    [SerializeField] ButtonBase _buttonHome;
    [SerializeField] ButtonBase _buttonContinue;

    public override void Init()
    {
        base.Init();

        _buttonHome.Init();
        _buttonContinue.Init();
        _buttonHome.onClick.AddListener(() => GameplayMediator.SetGameState(EGameplayState.Leave));
        _buttonContinue.onClick.AddListener(() => GameplayMediator.SetGameState(EGameplayState.Play));
    }
}
}