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
public class TabsPanelMenu : TabPanel
{
#region Fields

    [SerializeField] protected RectTransform _itemsContainer;

    float _itemShowDelay;
    LayoutGroup _layoutGroup;
    ContentSizeFitter _contentSizeFitter;
    IMenuFmodFactory _menuFmodFactory;
    Sequence _showSequence;

    protected List<MenuItemAnimation> _itemsAnims = new();

#endregion

#region Public methods

    public override async UniTask Init(CancellationToken cancelToken, object config = null)
    {
        _menuFmodFactory = config as IMenuFmodFactory;

        _layoutGroup = _itemsContainer.GetComponent<LayoutGroup>();
        _contentSizeFitter = _itemsContainer.GetComponent<ContentSizeFitter>();
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
            _itemsAnims.ForEach(item => item.ShowInstant());
        else
        {
            _showSequence.Rewind();
            await _showSequence.Play().AwaitForComplete(TweenCancelBehaviour.Complete, cancelToken);
        }
    }

    public override async UniTask Hide(bool skipAnimation, CancellationToken cancelToken)
    {
        if (skipAnimation)
            _itemsAnims.ForEach(item => item.HideInstant());
        else
            await UniTask.WhenAll(_itemsAnims.Select(view => view.GetHideAnim().ToUniTask(TweenCancelBehaviour.Complete, cancelToken)));

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

    protected void SetShowSequence()
    {
        var itemShowDelay = 0f;
        var eventInstancePitch = 0f;
        _showSequence = DOTween.Sequence()
                               .Pause()
                               .SetAutoKill(false);

        foreach (var item in _itemsAnims)
        {
            var pitch = eventInstancePitch;
            _showSequence.Insert(itemShowDelay, item.GetShowAnim())
                         .InsertCallback(itemShowDelay, () => PlayItemShowSound(pitch));
            
            itemShowDelay += ConstUIAnimation.ITEM_SPAWN_DELAY;
            eventInstancePitch += .1f;
        }
        return;

        void PlayItemShowSound(float pitch)
        {
            var itemAppearFmodEvent = _menuFmodFactory.GetItemAppearFmodEvent();
            itemAppearFmodEvent.SetParameter(ConstFMOD.ITEM_APPEAR_PITCH, pitch);
            itemAppearFmodEvent.start();
        }
    }

#endregion
}
}