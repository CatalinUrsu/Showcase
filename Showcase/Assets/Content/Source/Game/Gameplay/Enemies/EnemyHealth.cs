using IdleNumbers;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Game.Gameplay
{
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Slider _hpSlider;

    IdleNumber _hp;
    
    public bool IsDead => _hpSlider.value <= 0;

    public void Set(IdleNumber initHp, int lvl)
    {
        _hp = new IdleNumber(initHp * Mathf.Pow(ConstGameplay.ENEMY_HP_MULTIPLIER, lvl));

        _hpSlider.maxValue = (float)_hp.Value;
        _hpSlider.value = _hpSlider.maxValue;
    }

    public void TakeDamage(IdleNumber damage) => _hpSlider.value -= (float)damage.RoundToTargetValue(_hp.Lvl);
}
}