using System;
using Source.MVP;
using UnityEngine;
using FMOD.Studio;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.UI
{
public class TabPanelShopShips : TabPanelShop
{
    [Space]
    [SerializeField] ResetProgressView _resetButton;

    public override async UniTask InitContent(Action onFinishInit, EventInstance elementShowSound, CancellationTokenSource cts)
    {
        await base.InitContent(onFinishInit, elementShowSound, cts);

        _resetButton.Init();
        _elemntsAnimations.Insert(0, _resetButton.GetComponent<MenuElementAnimation>());
    }
}
}