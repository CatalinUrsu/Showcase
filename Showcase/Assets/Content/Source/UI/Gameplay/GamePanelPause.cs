using Zenject;
using UnityEngine;

namespace Source.UI.Gameplay
{
public class GamePanelPause : GamePanel
{
    [Space]
    [SerializeField] ButtonBase _buttonHome;
    [SerializeField] ButtonBase _buttonContinue;

    [Inject] IGameRunModelController _gameRunModelController;

    public override void Init()
    {
        base.Init();

        _buttonHome.Init();
        _buttonContinue.Init();
        _buttonHome.Btn.onClick.AddListener(() => _gameRunModelController.OnClickGoHome_raise());
        _buttonContinue.Btn.onClick.AddListener(() => _gameRunModelController.OnClickReturnToGame_raise());
    }
}
}