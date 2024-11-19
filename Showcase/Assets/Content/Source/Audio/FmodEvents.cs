using Helpers;
using FMODUnity;
using UnityEngine;

namespace Source.Audio
{
public class FmodEvents : Singleton<FmodEvents>
{
    [Header("General")]
    public EventReference BtnClick;
    public EventReference Music;
    public EventReference Fly;

    [Header("Menu")] 
    public EventReference BtnBuy;
    public EventReference BtnSelect;
    public EventReference ResetProgress;
    public EventReference ItemAppear;

    [Header("Gameplay")] 
    public EventReference Hit;
    public EventReference Shoot;
    public EventReference PlayerDeath;
    public EventReference EnemyDeath;
    public EventReference PopupOpen;
    public EventReference PopupClose;
    public EventReference LvlUp;
}
}