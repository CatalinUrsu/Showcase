using Zenject;
using Helpers;
using System.Linq;
using UnityEngine;
using Helpers.Services;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Source.UI.Gameplay
{
public class GameUIFacade : MonoBehaviour, IGameUIFacade
{
    [SerializeField] Canvas _canvasInputHandler;
    [SerializeField] Canvas _canvas;
    [SerializeField] GamePanel[] _panels;

    Dictionary<EGamePanels, GamePanel> _panelsByTypes;
    GamePanel _currentPanel;
    [Inject] ICameraService _cameraService;

    public void Init()
    {
        InputManager.Instance.OnToggleInputLock += OnToggleInputLock_handler;
        InitCanvas();
        InitPanelsByType();
    }

    public void Deinit()
    {
        InputManager.Instance.OnToggleInputLock -= OnToggleInputLock_handler;

        foreach (var panelByType in _panelsByTypes)
            panelByType.Value.Deinit();
    }

    public async UniTask SelectPanel(EGamePanels panelType)
    {
        if (_currentPanel.EPanelType == panelType) return;

        if (_currentPanel != null)
            await _currentPanel.Hide();

        _currentPanel = _panelsByTypes[panelType];
        await _currentPanel.Show();
    }

#region Private methods

    void InitCanvas()
    {
        _canvas.worldCamera = _cameraService.GetCameraByKey(ConstCameras.CAMERA_UI);
        _canvas.planeDistance = 1;
    }

    void InitPanelsByType()
    {
        _panelsByTypes = _panels.ToDictionary(panel => panel.EPanelType, panel => panel);
        foreach (var panelByType in _panelsByTypes)
            panelByType.Value.Init();
    }

    void OnToggleInputLock_handler(bool isLock) => _canvasInputHandler.overrideSorting = isLock;

#endregion
}
}