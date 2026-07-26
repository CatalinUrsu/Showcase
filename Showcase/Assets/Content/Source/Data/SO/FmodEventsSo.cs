using FMODUnity;
using UnityEngine;

namespace Source.Data
{
[CreateAssetMenu(fileName = "FmodEvents", menuName = "SO/Audio/Fmod Events", order = 0)]
public class FmodEventsSo : ScriptableObject
{
    [Header("General")]
    public EventReference BtnClick;
    public EventReference Music;
    public EventReference Fly;

    [Header("Menu")] 
    public EventReference BtnBuy;
    public EventReference SelectItem;
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