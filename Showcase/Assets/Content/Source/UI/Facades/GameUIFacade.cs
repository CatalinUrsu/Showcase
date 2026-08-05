using System;
using Zenject;
using Helpers;
using System.Linq;
using UnityEngine;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.UI.Gameplay
{
public class GameUIFacade : MonoBehaviour, IGameUIFacade
{
    [SerializeField] Canvas _canvasInputHandler;
    [SerializeField] Canvas _canvas;
    [SerializeField] PanelByType[] _panelsByTypes;

    [Inject] ICameraService _cameraService;

    public void Init()
    {
        InputManager.Instance.OnToggleInputLock += OnToggleInputLock_handler;
        
        _canvas.worldCamera = _cameraService.GetCameraByKey(ConstCameras.CAMERA_UI);
        _canvas.planeDistance = 1;
        
        foreach (var panelByType in _panelsByTypes) 
            panelByType.Panel.Init();
    }

    public void Deinit()
    {
        InputManager.Instance.OnToggleInputLock -= OnToggleInputLock_handler;
        
        foreach (var panelByType in _panelsByTypes) 
            panelByType.Panel.Deinit();
    }

    public async UniTask ShowPanel(EGamePanels panelType) => await GetPanelByType(panelType).Show();
    public async UniTask HidePanel(EGamePanels panelType) => await GetPanelByType(panelType).Hide();

    
#region Private methods
    
    GamePanel GetPanelByType(EGamePanels panelType) => _panelsByTypes.First(panelByTypetype => panelByTypetype.EPanelType.Equals(panelType)).Panel;

    void OnToggleInputLock_handler(bool isLock) => _canvasInputHandler.overrideSorting = isLock;

#endregion
    
#region Datas

    [Serializable]
    public class PanelByType
    {
        public EGamePanels EPanelType;
        public GamePanel Panel;
    }

#endregion
}
}