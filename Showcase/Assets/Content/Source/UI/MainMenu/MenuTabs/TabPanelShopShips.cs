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
        await base.Init(cancelToken, config);

        _resetButton.Init();
        _elemntsAnimations.Insert(0, _resetButton.GetComponent<MenuElementAnimation>());
    }
}
}