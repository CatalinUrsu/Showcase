using System;
using FMOD.Studio;
using UnityEngine;
using DG.Tweening;
using Source.Audio;
using Helpers.Audio;
using System.Threading;
using Source.StateMachine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Source.UI
{
public class TabsGroup : MonoBehaviour
{
#region Fields

    [SerializeField] MenuMediator _menuMediator;
    [SerializeField] RectTransform _indicatorRT;
    [SerializeField] List<Tab> _tabs = new();

    int _spawnFinish;
    bool _tabIsSwapping;
    EventInstance _tabElementSoundEvent;
    Tab _activeTab;
    CancellationTokenSource _cts;

#endregion

#region Public methods

    public async UniTask InitContent(Action<float> updateInitProgress)
    {
        _cts = new CancellationTokenSource();
        _menuMediator.OnClickStartGame += () => CancelTabSwappingTask().GetAwaiter();
        await InitTabs();

        TabBtnClick_handler(_tabs[0], true).Forget();

        async UniTask InitTabs()
        {
            foreach (var tab in _tabs)
            {
                tab.TabBtn.Init();
                tab.TabBtn.onClick.AddListener(() => TabBtnClick_handler(tab).Forget());
            }

            _tabElementSoundEvent = FmodEvents.Instance.ItemAppear.GetInstance(ConstSceneNames.MENU_SCENE);
            await UniTask.WhenAll(_tabs.Select(tab => tab.TabPanel.InitContent(() => updateInitProgress.Invoke(1f / _tabs.Count), _tabElementSoundEvent, _cts)));
        }
    }

#endregion

#region Private methods
    
    async UniTaskVoid TabBtnClick_handler(Tab selectedTab, bool skipAnimation = false)
    {
        if (_activeTab == selectedTab) return;
        
        await CancelTabSwappingTask();
        
        if (_cts.IsCancellationRequested)
            _cts = new CancellationTokenSource();
        
        SwapTabs(selectedTab, skipAnimation, _cts).Forget();
    }

    async UniTaskVoid SwapTabs(Tab selectedTab, bool skipAnimation, CancellationTokenSource cts = default)
    {
        _tabIsSwapping = true;
        var tasksBeforeShowNewTab = new List<UniTask>();

        //Deselect and hide previous tab 
        if (_activeTab != null)
        {
            tasksBeforeShowNewTab.Add(_activeTab.TabBtn.Deselect(skipAnimation, cts));
            tasksBeforeShowNewTab.Add(_activeTab.TabPanel.Hide(skipAnimation, cts));
        }

        //Move indicator and activate new tab button
        _activeTab = selectedTab;
        tasksBeforeShowNewTab.Add(GetIndicatorTweenTask(skipAnimation, cts));
        tasksBeforeShowNewTab.Add(selectedTab.TabBtn.Select(skipAnimation, cts));

        await UniTask.WhenAll(tasksBeforeShowNewTab);
        await selectedTab.TabPanel.Show(skipAnimation, cts);

        _tabIsSwapping = false;

        UniTask GetIndicatorTweenTask(bool skipAnimation, CancellationTokenSource cts) =>
            _indicatorRT.DOMoveX(_activeTab.TabBtn._rtRef.position.x, ConstUIAnimation.GetAnimDuration(skipAnimation))
                        .ToUniTask(TweenCancelBehaviour.Complete, cts.Token);
    }

    async UniTask CancelTabSwappingTask()
    {
        if (!_tabIsSwapping || _cts.IsCancellationRequested) return;

        _cts.Cancel();
        while (_tabIsSwapping)
            await UniTask.Yield();

    }

#endregion

#region Tab Class

    [Serializable]
    public class Tab
    {
        public ButtonTab TabBtn;
        public TabPanel TabPanel;
    }

#endregion
}
}