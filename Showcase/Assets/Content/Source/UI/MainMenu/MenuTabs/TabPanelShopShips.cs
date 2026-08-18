using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.UI
{
public class TabPanelShopShips : TabPanelShop
{
    [Space]
    [SerializeField] ResetProgressView _resetButton;

    public override async UniTask Init(CancellationToken cancelToken, object config = null)
    {
        _itemsAnims.Insert(0, _resetButton.GetComponent<MenuItemAnimation>());
        _resetButton.Init();

        await base.Init(cancelToken, config);
    }
}
}