using IdleNumbers;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Game.Gameplay
{
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Slider _hpSlider;

    IdleNumber _maxHp;
    IdleNumber _currentHp;

    public bool IsDead => _currentHp.Value <= 0;

    public void Set(IdleNumber initHp, int lvl, float hpMultiplier)
    {
        _maxHp = new IdleNumber(initHp * Mathf.Pow(hpMultiplier, lvl));
        _currentHp = _maxHp;

        _hpSlider.maxValue = 1;
        _hpSlider.value = 1;
    }

    public void TakeDamage(IdleNumber damage)
    {
        _currentHp = _currentHp.IsEnough(damage) ? _currentHp - damage : new IdleNumber();

        // Slider only shows the share: current HP in units of max HP order, divided by max
        _hpSlider.value = (float)(_currentHp.RoundToTargetValue(_maxHp.Lvl) / _maxHp.Value);
    }
}
}
