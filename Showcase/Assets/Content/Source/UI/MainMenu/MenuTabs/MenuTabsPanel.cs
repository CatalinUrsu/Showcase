using System;
using Helpers.UI;
using DG.Tweening;
using UnityEngine;
using Helpers.Audio;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Source.UI
{
[RequireComponent(typeof(LayoutGroup), typeof(ContentSizeFitter))]
public class MenuTabsPanel : TabPanel
{
#region Fields

    [SerializeField] protected RectTransform _itemsContainer;

    float _itemShowDelay;
    LayoutGroup _layoutGroup;
    ContentSizeFitter _contentSizeFitter;
    IMenuTabsGroup _menuTabsGroup;
    Sequence _showSequence;

    protected List<MenuElementAnimation> _elemntsAnimations = new();

#endregion

#region Public methods

    public override async UniTask Init(CancellationToken cancelToken, object config = null)
    {
        _menuTabsGroup = config as IMenuTabsGroup;

        _layoutGroup = _itemsContainer.GetComponent<LayoutGroup>();
        _contentSizeFitter = _itemsContainer.GetComponent<ContentSizeFitter>();

        SetShowSequence();
        await UniTask.CompletedTask;
    }

    public override void Deinit()
    {
        if (_showSequence == null) return;

        _showSequence.Kill(true);
        _showSequence = null;
    }

    public override async UniTask Show(bool skipAnimation, CancellationToken cancelToken)
    {
        gameObject.SetActive(true);

        if (skipAnimation)
            _elemntsAnimations.ForEach(item => item.ShowInstant());
        else
            await _showSequence.Play().ToUniTask(TweenCancelBehaviour.Complete, cancelToken);
    }

    public override async UniTask Hide(bool skipAnimation, CancellationToken cancelToken)
    {
        if (skipAnimation)
            _elemntsAnimations.ForEach(item => item.HideInstant());
        else
            await UniTask.WhenAll(_elemntsAnimations.Select(view => view.GetHideAnim().ToUniTask(TweenCancelBehaviour.Complete, cancelToken)));

        gameObject.SetActive(false);
    }

#endregion

#region Private methods

    protected async UniTask SetLayoutComponents(CancellationToken cancelToken)
    {
        try
        {
            gameObject.SetActive(true);
            _contentSizeFitter.enabled = true;
            await UniTask.Yield(cancelToken);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_itemsContainer);
            await UniTask.Yield(cancelToken);

            _contentSizeFitter.enabled = false;
            _layoutGroup.enabled = false;
            await UniTask.Yield(cancelToken);

            await Hide(true, cancelToken);
        }
        catch (OperationCanceledException) when (cancelToken.IsCancellationRequested)
        {
            _contentSizeFitter.enabled = false;
            _layoutGroup.enabled = false;

            gameObject.SetActive(false);
        }
    }

    void SetShowSequence()
    {
        var itemShowDelay = 0f;
        var eventInstancePitch = 0f;
        _showSequence = DOTween.Sequence()
                               .Pause()
                               .SetAutoKill(false);

        foreach (var item in _elemntsAnimations)
        {
            var showTween = item.GetShowAnim()
                                .OnStart(() =>
                                {
                                    var itemAppearFmodEvent = _menuTabsGroup.GetItemAppearFmodEvent();
                                    itemAppearFmodEvent.SetParameter(ConstFMOD.ITEM_APPEAR_PITCH, eventInstancePitch);
                                    itemAppearFmodEvent.start();

                                    eventInstancePitch += .1f;
                                });

            _showSequence.Insert(itemShowDelay, showTween);
            itemShowDelay += ConstUIAnimation.ITEM_SPAWN_DELAY;
        }
    }

#endregion
}
}