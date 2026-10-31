using R3;
using System;
using Zenject;
using Source.Data;
using Helpers.Audio;

namespace Source.Services
{
/// <summary>
/// Effects of shop and ascend results: sound and save.
/// </summary>
public class ShopFeedback : IInitializable, IDisposable
{
#region Fields

    readonly IShopService _shopService;
    readonly IProgressionService _progressionService;
    readonly FmodEventsSo _fmodEvents;
    readonly ISessionService _sessionService;
    readonly CompositeDisposable _subscriptions = new();

#endregion

#region Public methods

    public ShopFeedback(IShopService shopService,
                        IProgressionService progressionService,
                        FmodEventsSo fmodEvents,
                        ISessionService sessionService)
    {
        _shopService = shopService;
        _progressionService = progressionService;
        _fmodEvents = fmodEvents;
        _sessionService = sessionService;
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

        SaveProgress();
    }

    void OnAscended()
    {
        _fmodEvents.ResetProgress.PlayOneShot();
        SaveProgress();
    }

    // shortcut: saves on every action, replaced by SaveService dirty flag in W5
    void SaveProgress()
    {
        _sessionService.Save(ESaveFileType.Items);
        _sessionService.Save(ESaveFileType.Progress);
    }

#endregion
}
}
