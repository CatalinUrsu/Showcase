using R3;
using System;
using UnityEngine;

namespace Source.Data
{
/// <summary>
/// Ascend (reset progress for diamonds): level and coins reset, weapons reset, ships stay.
/// </summary>
public class ProgressionService : IProgressionService, IDisposable
{
#region Fields

    public Observable<Unit> Ascended => _ascended;

    public bool CanAscend => _progress.Lvl.CurrentValue >= _balance.ResetProgressMinLvl;
    public int AscendBonus => Mathf.RoundToInt(_progress.Lvl.CurrentValue * _balance.ResetProgressMultiplier);
    public int AscendMinLvl => _balance.ResetProgressMinLvl;

    readonly ProgressModel _progress;
    readonly ItemsModel _items;
    readonly GameBalanceSO _balance;
    readonly Subject<Unit> _ascended = new();

#endregion

#region Public methods

    public ProgressionService(ProgressModel progress, ItemsModel items, GameBalanceSO balance)
    {
        _progress = progress;
        _items = items;
        _balance = balance;
    }

    public bool TryAscend()
    {
        if (!CanAscend) return false;

        _progress.Ascend(AscendBonus);
        _items.ResetOnAscending();

        _ascended.OnNext(Unit.Default);
        return true;
    }

    public void Dispose() => _ascended.Dispose();

#endregion
}
}
