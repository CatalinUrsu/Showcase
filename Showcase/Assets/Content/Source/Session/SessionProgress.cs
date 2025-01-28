using R3;
using System;
using IdleNumbers;

namespace Source.Session
{
[Serializable]
public class SessionProgress
{
    public ReactiveProperty<int> Lvl { get; private set; }
    public ReactiveProperty<IdleNumber> Coins { get; private set; }
    public ReactiveProperty<IdleNumber> Diamonds { get; private set; }
    public ReactiveProperty<int> UsedShipIdx { get; private set; }
    public ReactiveProperty<int> UsedWeaponIdx { get; private set; }

    public SessionProgress()
    {
        Lvl = new ReactiveProperty<int>(1);
        Coins = new ReactiveProperty<IdleNumber>(new IdleNumber());
        Diamonds = new ReactiveProperty<IdleNumber>(new IdleNumber());
        UsedShipIdx = new ReactiveProperty<int>(0);
        UsedWeaponIdx = new ReactiveProperty<int>(0);
    }

    public SessionProgress(SessionProgress deserializeObject)
    {
        Lvl = deserializeObject.Lvl;
        Coins = deserializeObject.Coins;
        Diamonds = deserializeObject.Diamonds;
        UsedShipIdx = deserializeObject.UsedShipIdx;
        UsedWeaponIdx = deserializeObject.UsedWeaponIdx;
    }
}
}