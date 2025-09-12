using System;
using Zenject;
using System.Linq;
using UnityEngine;
using Source.Gameplay;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.UI.Gameplay
{
public class GameUIController : MonoBehaviour
{
    [SerializeField] PanelByType[] _panelsByTypes;
    [SerializeField] Canvas _canvas;

    [Inject] IServiceCamera _serviceCamera;
    
    public void Init()
    {
        _canvas.worldCamera = _serviceCamera.GetCameraByKey(ConstCameras.CAMERA_UI);
        _canvas.planeDistance = 1;
        
        foreach (var panelByType in _panelsByTypes) 
            panelByType.Panel.Init();
    }

    public void Deinit()
    {
        foreach (var panelByType in _panelsByTypes) 
            panelByType.Panel.Deinit();
    }

    public async UniTask ShowPanel(EGamePanels panelType) => await GetPanelByType(panelType).Show();
    public async UniTask HidePanel(EGamePanels panelType) => await GetPanelByType(panelType).Hide();

    GamePanel GetPanelByType(EGamePanels panelType) => _panelsByTypes.First(panelByTypetype => panelByTypetype.EPanelType.Equals(panelType)).Panel;
    
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