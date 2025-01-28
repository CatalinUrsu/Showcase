using R3;
using UnityEngine;
using Source.Audio;
using Helpers.Audio;
using Source.Session;

namespace Source.MVP
{
public class ResetProgressPresenter
{
    IViewResetProgress _resetProgressView;

    public ResetProgressPresenter(GameObject resetProgressGO, IViewResetProgress resetProgressView)
    {
        _resetProgressView = resetProgressView;

        SessionManager.Current.Progress.Lvl.Subscribe(OnChangeLvl_handler).AddTo(resetProgressGO);
    }

    public void OnClick_handler()
    {
        if (!ReachedMinBonusLvl) return;

        var itemModelWeapons = SessionManager.Current.Items.Weapons;

        FmodEvents.Instance.ResetProgress.PlayOneShot();
        SessionManager.Current.Progress.Coins.Value *= 0;
        SessionManager.Current.Progress.Diamonds.Value += LvlBonus;
        SessionManager.Current.Progress.UsedWeaponIdx.Value = 0;
        foreach (var weaponModel in itemModelWeapons) 
            weaponModel.Value.ResetModel();
        
        SessionManager.Current.Progress.Lvl.Value = 1;
        SessionManager.Current.Save(ESaveFileType.Items);
        SessionManager.Current.Save(ESaveFileType.Progress);
    }

    void OnChangeLvl_handler(int lvl)
    {
        _resetProgressView.OnChangeLvl_handler(ReachedMinBonusLvl, LvlBonus);
    }

    int LvlBonus => Mathf.RoundToInt(SessionManager.Current.Progress.Lvl.Value * ConstUpgradeItems.RESET_PROGRESS_MULTIPLIER);
    bool ReachedMinBonusLvl => SessionManager.Current.Progress.Lvl.Value >= ConstUpgradeItems.RESET_PROGRESS_MIN_LVL;
}
}