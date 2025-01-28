using R3;
using System;
using Source.MVP;
using UnityEngine;
using DG.Tweening;
using FMOD.Studio;
using Source.SO.Items;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Source.UI
{
public class TabPanelShop : TabPanel
{
#region Fields

    [SerializeField] ItemView _item;
    [SerializeField] ItemLookSO[] _itemAppearenceSO;

    protected List<ItemView> _items = new();
    float _effectLocation;
    Tween _shineTween;

#endregion

#region Public methods

    public override async UniTask InitContent(Action onFinishInit, EventInstance elementShowSound, CancellationTokenSource cts)
    {
        await base.InitContent(onFinishInit, elementShowSound, cts);
        
        InitItems();
        SetItemsShineEffect();
        
        await SetLayoutComponents(onFinishInit, cts);
    }

#endregion

#region Private methods

    void InitItems()
    {
        for (var i = 0; i < _itemAppearenceSO.Length; i++)
        {
            var itemAppearence = _itemAppearenceSO[i];
            var item = Instantiate(_item, _itemsContainer);
            item.Init(itemAppearence, i);
            _items.Add(item);
            _elemntsAnimations.Add(item.GetComponent<MenuElementAnimation>());
        }
    }

    void SetItemsShineEffect()
    {
        _shineTween = DOTween.To(() => 0f, x => _effectLocation = x, 1f, 1f)
                             .SetAutoKill(false)
                             .OnUpdate(() =>
                             {
                                 foreach (var item in _items) 
                                     item.UpdateShineEffect(_effectLocation);
                             })
                             .Pause();

        Observable.Interval(TimeSpan.FromSeconds(2))
                  .Where(_ => gameObject.activeSelf)
                  .Do(onDispose: () => _shineTween.Kill())
                  .Subscribe(_ => _shineTween.Restart())
                  .AddTo(this);
    }

#endregion
}
}