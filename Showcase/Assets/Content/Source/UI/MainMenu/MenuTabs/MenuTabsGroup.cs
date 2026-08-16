using System;
using Helpers.UI;
using UnityEngine;
using DG.Tweening;
using Source.Data;
using FMOD.Studio;
using Helpers.Audio;
using System.Threading;
using Helpers.PoolSystem;
using Cysharp.Threading.Tasks;

namespace Source.UI
{
public class MenuTabsGroup : TabsGroup, IMenuTabsGroup
{
    [Space]
    [SerializeField] RectTransform _indicatorRT;
    [SerializeField] FmodEventsSo _fmodEventsSo;

    Pool<EventInstance> _factoryFmodEvents;

    public override UniTask Init(Action<float> onInitUpdate)
    {
        _factoryFmodEvents = new FactoryFmodEvents.Builder(_fmodEventsSo.ItemAppear)
                             .SetMaxCount(5)
                             .Build();

        return base.Init(onInitUpdate);
    }

    public override async UniTask Deinit()
    {
        await base.Deinit();

        _factoryFmodEvents.Clear();
    }

    public EventInstance GetItemAppearFmodEvent() => _factoryFmodEvents.Get();

    protected override void SetTabsSwapTasks(Tab selectedTab, bool skipAnimation)
    {
        base.SetTabsSwapTasks(selectedTab, skipAnimation);
        _tabsSwapTasks.Add(GetIndicatorTweenTask(skipAnimation, _swapCts));
    }

    UniTask GetIndicatorTweenTask(bool skipAnimation, CancellationTokenSource cts) =>
        _indicatorRT.DOMoveX(_activeTab.TabBtn.BtnHelper.RT.position.x, ConstUIAnimation.GetAnimDuration(skipAnimation))
                    .ToUniTask(TweenCancelBehaviour.Complete, cts.Token);
}
}