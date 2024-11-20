using System;
using Helpers;
using DG.Tweening;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Source.UI
{
public class TabPanel : MonoBehaviour
{
#region Fields

    [SerializeField] protected RectTransform _itemsContainer;

    float _itemShowDelay;
    LayoutGroup _layoutGroup;
    EventInstance _elementShowSound;
    ContentSizeFitter _contentSizeFitter;

    protected List<MenuElementAnimation> _elemntsAnimations = new();
    protected float _eventInstancePitch;
    protected Sequence _showSequence;

#endregion

#region Public methods

    public virtual async UniTask InitContent(Action onFinishInit, EventInstance elementShowSound, CancellationTokenSource cts)
    {
        _elementShowSound = elementShowSound;
        await UniTask.CompletedTask;
    }

    public async UniTask Show(bool skipAnimation, CancellationTokenSource cts)
    {
        PreparePanelToShow();

        foreach (var item in _elemntsAnimations)
            AddShowItemToSequence(_showSequence, item, skipAnimation);

        await _showSequence.Play().ToUniTask(TweenCancelBehaviour.Complete, cts.Token);
    }

    public async UniTask Hide(bool skipAnimation, CancellationTokenSource cts)
    {
        var itemsHideTasks = _elemntsAnimations.Select(view => view.GetHideAnim(skipAnimation, cts.Token));
        await UniTask.WhenAll(itemsHideTasks);

        gameObject.SetActive(false);
    }
    

#endregion

#region Protected methods

    /// <summary>
    ///Force rebuild layout to avoid some weird object placement
    ///After rebuild, disable Components to optimize UI and instantly hide panel
    /// </summary>
    protected async UniTask SetLayoutComponents(Action onFinishInit, CancellationTokenSource cts)
    {
        gameObject.SetActive(true);
        _layoutGroup = _itemsContainer.GetComponent<LayoutGroup>();
        _contentSizeFitter = _itemsContainer.GetComponent<ContentSizeFitter>();

        _contentSizeFitter.enabled = true;
        await UniTask.Yield();

        LayoutRebuilder.ForceRebuildLayoutImmediate(_itemsContainer);
        await UniTask.Yield();

        _contentSizeFitter.enabled = false;
        _layoutGroup.enabled = false;
        await UniTask.Yield();

        onFinishInit?.Invoke();
        await Hide(true, cts);
    }

    /// <summary>
    /// Reset panel's elements show delay and sound pitch before start to show panel
    /// </summary>
    protected void PreparePanelToShow()
    {
        gameObject.SetActive(true);
        _itemShowDelay = 0;
        _eventInstancePitch = 0f;
        _showSequence = DOTween.Sequence().Pause();
        
        _elementShowSound.SetParameter(ConstFMOD.ITEM_APPEAR_PITCH, _eventInstancePitch);
    }

    /// <summary>
    /// Get ShowTween of some element on panel and insert into sequence (like waterfall animation)
    /// </summary>
    /// <param name="sequence">Sequence to add tween</param>
    /// <param name="elementAnimation">Selected element with show tween (animation)</param>
    /// <param name="skipAnimation">Check if need to skip Element's animation, don't play audio, don't change delay (Useful for Init)</param>
    /// <param name="delay">position of Element's Tween in Sequence</param>
    protected void AddShowItemToSequence(Sequence sequence, MenuElementAnimation elementAnimation, bool skipAnimation)
    {
        var elementShowTween = elementAnimation.GetShowAnim(skipAnimation)
                                      .OnStart(() =>
                                      {
                                          if (skipAnimation) return;

                                          _elementShowSound.start();
                                          _elementShowSound.SetParameter(ConstFMOD.ITEM_APPEAR_PITCH, _eventInstancePitch);
                                          _eventInstancePitch += .1f;
                                      });

        sequence.Insert(_itemShowDelay, elementShowTween);
        _itemShowDelay += skipAnimation ? 0 : ConstUIAnimation.ITEM_SPAWN_DELAY;
    }

#endregion
}
}