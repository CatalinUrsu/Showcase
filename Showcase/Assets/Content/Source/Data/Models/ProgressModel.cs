using R3;
using System;
using IdleNumbers;

namespace Source.Data
{
[Serializable]
public class ProgressModel : IProgressModel
{
    // ---------- Runtime ----------
    public ReadOnlyReactiveProperty<int> LvlRef => Lvl;
    public ReadOnlyReactiveProperty<IdleNumber> CoinsRef => Coins;
    public ReadOnlyReactiveProperty<IdleNumber> DiamondsRef => Diamonds;
    public ReadOnlyReactiveProperty<int> UsedShipIdxRef => UsedShipIdx;
    public ReadOnlyReactiveProperty<int> UsedWeaponIdxRef => UsedWeaponIdx;

    // ---------- Serialized ----------
    public ReactiveProperty<int> Lvl { get; private set; }
    public ReactiveProperty<IdleNumber> Coins { get; private set; }
    public ReactiveProperty<IdleNumber> Diamonds { get; private set; }
    public ReactiveProperty<int> UsedShipIdx { get; private set; }
    public ReactiveProperty<int> UsedWeaponIdx { get; private set; }
}
}