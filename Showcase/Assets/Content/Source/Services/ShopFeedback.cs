using R3;
using System;
using Zenject;
using Source.Data;
using Helpers.Audio;

namespace Source.Services
{
/// <summary>
/// Sound feedback of shop and ascend results.
/// </summary>
public class ShopFeedback : IInitializable, IDisposable
{
#region Fields

    readonly IShopService _shopService;
    readonly IProgressionService _progressionService;
    readonly FmodEventsSo _fmodEvents;
    readonly CompositeDisposable _subscriptions = new();

#endregion

#region Public methods

    public ShopFeedback(IShopService shopService,
                        IProgressionService progressionService,
                        FmodEventsSo fmodEvents)
    {
        _shopService = shopService;
        _progressionService = progressionService;
        _fmodEvents = fmodEvents;
    }

    public void Initialize()
    {
        _shopService.Actions.Subscribe(OnShopAction).AddTo(_subscriptions);
        _progressionService.Ascended.Subscribe(_ => OnAscended()).AddTo(_subscriptions);
    }

    public void Dispose() => _subscriptions.Dispose();

#endregion

#region Private methods

    void OnShopAction(EShopAction action)
    {
        if (action == EShopAction.Selected)
            _fmodEvents.SelectItem.PlayOneShot();
        else
            _fmodEvents.BtnBuy.PlayOneShot();
    }

    void OnAscended() => _fmodEvents.ResetProgress.PlayOneShot();

#endregion
}
}
