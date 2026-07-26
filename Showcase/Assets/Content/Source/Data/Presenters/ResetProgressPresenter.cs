using UnityEngine;

namespace Source.Data.MVP.Presenters
{
public class ResetProgressPresenter
{
    IViewResetProgress _resetProgressView;

    public ResetProgressPresenter(GameObject resetProgressGO, IViewResetProgress resetProgressView)
    {
        _resetProgressView = resetProgressView;

        SessionService.Current.Progress.Lvl.Subscribe(OnChangeLvl_handler).AddTo(resetProgressGO);
    }

    public void OnClick_handler()
    {
        if (!ReachedMinBonusLvl) return;

        var itemModelWeapons = SessionService.Current.Items.Weapons;

        FmodEventsSo.Instance.ResetProgress.PlayOneShot();
        SessionService.Current.Progress.Coins.Value *= 0;
        SessionService.Current.Progress.Diamonds.Value += LvlBonus;
        SessionService.Current.Progress.UsedWeaponIdx.Value = 0;
        foreach (var weaponModel in itemModelWeapons) 
            weaponModel.Value.ResetModel();
        
        SessionService.Current.Progress.Lvl.Value = 1;
        SessionService.Current.Save(ESaveFileType.Items);
        SessionService.Current.Save(ESaveFileType.Progress);
    }

    void OnChangeLvl_handler(int lvl)
    {
        _resetProgressView.OnChangeLvl_handler(ReachedMinBonusLvl, LvlBonus);
    }

    int LvlBonus => Mathf.RoundToInt(SessionService.Current.Progress.Lvl.Value * ConstUpgradeItems.RESET_PROGRESS_MULTIPLIER);
    bool ReachedMinBonusLvl => SessionService.Current.Progress.Lvl.Value >= ConstUpgradeItems.RESET_PROGRESS_MIN_LVL;
}
}