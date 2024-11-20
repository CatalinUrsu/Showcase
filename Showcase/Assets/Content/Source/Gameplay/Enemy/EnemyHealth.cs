using UnityEngine;
using IdleNumbers;
using Source.Session;
using UnityEngine.UI;

namespace Source.Gameplay
{
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Slider _hpSlider;

    int _hpIdleNumLvl;
    
    public bool IsDead => _hpSlider.value <= 0;

    public void Set(IdleNumber initEnemyHP)
    {
        var idleNumberHP = new IdleNumber(initEnemyHP) * Mathf.Pow(ConstGameplay.ENEMY_HP_MULTIPLIER, SessionManager.Current.Progress.Lvl.Value);
        _hpIdleNumLvl = idleNumberHP.Lvl;

        _hpSlider.maxValue = (float)idleNumberHP.Value;
        _hpSlider.value = _hpSlider.maxValue;
    }

    public void OnHit_handler(IdleNumber damage)
    {
        _hpSlider.value -= (float)damage.RoundToTargetValue(_hpIdleNumLvl);
    }
}
}