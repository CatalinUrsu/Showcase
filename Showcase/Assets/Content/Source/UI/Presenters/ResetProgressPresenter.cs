using R3;
using System;
using Zenject;
using Source.Data;

namespace Source.UI
{
public class ResetProgressPresenter : IDisposable
{
#region External Types

    public class Factory : PlaceholderFactory<IResetProgressView, ResetProgressPresenter> { }

#endregion

#region Fields

    readonly IProgressionService _progressionService;
    readonly CompositeDisposable _disposables = new();

#endregion

#region Public methods

    public ResetProgressPresenter(IResetProgressView view, IProgressionService progressionService, ProgressModel progress)
    {
        _progressionService = progressionService;

        progress.Lvl.Subscribe(_ => view.OnChangeLvl_handler(progressionService.CanAscend,
                                                            progressionService.AscendBonus,
                                                            progressionService.AscendMinLvl))
                .AddTo(_disposables);
    }

    public void TryAscend() => _progressionService.TryAscend();

    public void Dispose() => _disposables.Dispose();

#endregion
}
}
