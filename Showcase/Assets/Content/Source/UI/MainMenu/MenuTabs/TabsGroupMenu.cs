using System;
using Helpers.UI;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Source.UI
{
public class TabsGroupMenu : TabsGroup
{
    [Space]
    [SerializeField] RectTransform _indicatorRT;
    [SerializeField] MenuFmodFactory _menuFmodFactory;

    public override UniTask Init(Action<float> onInitUpdate)
    {
        _menuFmodFactory.Init();
        return base.Init(onInitUpdate);
    }

    public override async UniTask Deinit()
    {
        await base.Deinit();

        _menuFmodFactory.Deinit();
    }

    protected override UniTask InitItem(Tab tab) => tab.TabPanel.Init(_initCts.Token, _menuFmodFactory);

    protected override void SetTabsSwapTasks(Tab selectedTab, bool skipAnimation)
    {
        base.SetTabsSwapTasks(selectedTab, skipAnimation);
        _tabsSwapTasks.Add(GetIndicatorTweenTask(selectedTab, skipAnimation));
    }

    UniTask GetIndicatorTweenTask(Tab selectedTab, bool skipAnimation) =>
        _indicatorRT.DOMoveX(selectedTab.TabBtn.BtnHelper.RT.position.x, ConstUIAnimation.GetAnimDuration(skipAnimation))
                    .ToUniTask(TweenCancelBehaviour.Complete, _swapCts.Token);
}
}