using R3;
using System;
using Zenject;
using UnityEngine;
using DG.Tweening;
using Source.Data;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Source.UI
{
public class TabPanelShop : TabsPanelMenu
{
#region Fields

    [SerializeField] ItemView _item;
    [SerializeField] ItemLookSO[] _itemAppearenceSO;

    protected List<ItemView> _items = new();
    float _effectLocation;
    Tween _shineTween;
    [Inject] DiContainer _diContainer;

#endregion

#region Public methods

    public override async UniTask Init(CancellationToken cancelToken, object config = null)
    {
        await base.Init(cancelToken, config);

        InitItems();
        SetShowSequence();
        SetItemsShineEffect();

        await SetLayoutComponents(cancelToken);
    }

#endregion

#region Private methods

    void InitItems()
    {
        for (var i = 0; i < _itemAppearenceSO.Length; i++)
        {
            var itemAppearance = _itemAppearenceSO[i];
            var itemView = _diContainer.InstantiatePrefabForComponent<ItemView>(_item, _itemsContainer);

            itemView.Init(itemAppearance);
            _items.Add(itemView);
            _itemsAnims.Add(itemView.GetComponent<MenuItemAnimation>());
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