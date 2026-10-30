using R3;
using System;
using Zenject;
using FMODUnity;
using UnityEngine;
using Helpers.Audio;

namespace Source.Data
{
public class ResetProgressPresenter : IDisposable
{
#region External Types

    public class Factory : PlaceholderFactory<IResetProgressView, ResetProgressPresenter> { }

#endregion
    
#region Fields

    readonly IResetProgressView _resetProgressView;
    readonly IProgressModelController _progressModelController;
    readonly IItemsModelController _itemsModelController;
    readonly ISessionService _sessionService;
    readonly GameBalanceSO _balance;
    readonly EventReference _fmodEvent;
    readonly CompositeDisposable _disposables = new();

    int LvlBonus => Mathf.RoundToInt(_progressModelController.IModel.LvlRef.CurrentValue * _balance.ResetProgressMultiplier);
    bool ReachedMinBonusLvl => _progressModelController.IModel.LvlRef.CurrentValue >= _balance.ResetProgressMinLvl;

#endregion

#region Public methods

    public ResetProgressPresenter(IResetProgressView resetProgressView,
                                  IProgressModelController progressModelController,
                                  IItemsModelController itemsModelController,
                                  ISessionService sessionService,
                                  FmodEventsSo fmodEventsSo,
                                  GameBalanceSO balance)
    {
        _resetProgressView = resetProgressView;
        _progressModelController = progressModelController;
        _itemsModelController = itemsModelController;
        _sessionService = sessionService;
        _balance = balance;
        _fmodEvent = fmodEventsSo.ResetProgress;

        _progressModelController.IModel.LvlRef.Subscribe(OnChangeLvl_handler).AddTo(_disposables);
    }

    public void Dispose() => _disposables.Dispose();

    public void TryAscend()
    {
        if (!ReachedMinBonusLvl) return;

        _fmodEvent.PlayOneShot();
        _progressModelController.AscendProgress(LvlBonus);
        _itemsModelController.ResetOnAscending();
        SaveProgress();
    }

#endregion

#region Private methods

    void OnChangeLvl_handler(int lvl) => _resetProgressView.OnChangeLvl_handler(ReachedMinBonusLvl, LvlBonus, _balance.ResetProgressMinLvl);

    void SaveProgress()
    {
        _sessionService.Save(ESaveFileType.Items);
        _sessionService.Save(ESaveFileType.Progress);
    }

#endregion
}
}