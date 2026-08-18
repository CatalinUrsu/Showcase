using R3;
using System;
using IdleNumbers;
using UnityEngine;
using System.Text.Json.Serialization;

namespace Source.Data
{
[Serializable]
public class ProgressModel : IProgressModel
{
    // ---------- Runtime ----------
    [JsonIgnore] public ReadOnlyReactiveProperty<int> LvlRef => Lvl;
    [JsonIgnore] public ReadOnlyReactiveProperty<IdleNumber> CoinsRef => Coins;
    [JsonIgnore] public ReadOnlyReactiveProperty<IdleNumber> DiamondsRef => Diamonds;
    [JsonIgnore] public ReadOnlyReactiveProperty<int> UsedShipIdxRef => UsedShipIdx;
    [JsonIgnore] public ReadOnlyReactiveProperty<int> UsedWeaponIdxRef => UsedWeaponIdx;

    // ---------- Serialized ----------
    [JsonInclude] public ReactiveProperty<int> Lvl { get; private set; }
    [JsonInclude] public ReactiveProperty<IdleNumber> Coins { get; private set; }
    [JsonInclude] public ReactiveProperty<IdleNumber> Diamonds { get; private set; }
    [JsonInclude] public ReactiveProperty<int> UsedShipIdx { get; private set; }
    [JsonInclude] public ReactiveProperty<int> UsedWeaponIdx { get; private set; }

    public ProgressModel()
    {
        Debug.Log("ProgressModel: constructor");
        Lvl = new ReactiveProperty<int>(1);
        Coins = new ReactiveProperty<IdleNumber>(new IdleNumber());
        Diamonds = new ReactiveProperty<IdleNumber>(new IdleNumber());
        UsedShipIdx = new ReactiveProperty<int>(0);
        UsedWeaponIdx = new ReactiveProperty<int>(0);
    }
}
}