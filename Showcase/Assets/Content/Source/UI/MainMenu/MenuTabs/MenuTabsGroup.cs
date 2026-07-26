using UnityEngine;
using DG.Tweening;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.UI
{
public class MenuTabsGroup : Helpers.UI.TabsGroup
{
    [SerializeField] RectTransform _indicatorRT;

    protected override void SetTabsSwapTasks(Tab selectedTab, bool skipAnimation)
    {
        base.SetTabsSwapTasks(selectedTab, skipAnimation);
        _tabsSwapTasks.Add(GetIndicatorTweenTask(skipAnimation, _cts));
    }

    UniTask GetIndicatorTweenTask(bool skipAnimation, CancellationTokenSource cts) =>
        _indicatorRT.DOMoveX(_activeTab.TabBtn.BtnHelper.RT.position.x, ConstUIAnimation.GetAnimDuration(skipAnimation))
                    .ToUniTask(TweenCancelBehaviour.Complete, cts.Token);
}
}