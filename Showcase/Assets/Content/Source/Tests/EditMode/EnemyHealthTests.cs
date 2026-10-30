using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Source.Game.Gameplay;

namespace Source.Tests
{
public class EnemyHealthTests
{
    GameObject _go;
    EnemyHealth _health;
    Slider _slider;

    [SetUp]
    public void SetUp()
    {
        _go = new GameObject("Enemy");
        _slider = _go.AddComponent<Slider>();
        _health = _go.AddComponent<EnemyHealth>();

        var serialized = new SerializedObject(_health);
        serialized.FindProperty("_hpSlider").objectReferenceValue = _slider;
        serialized.ApplyModifiedPropertiesWithoutUndo();

        _health.Set(100, lvl: 0, hpMultiplier: 1);
    }

    [TearDown]
    public void TearDown() => Object.DestroyImmediate(_go);

    [Test]
    public void TakeDamage_ThreeHitsOf40AtHp100_IsDead()
    {
        _health.TakeDamage(40);
        _health.TakeDamage(40);
        _health.TakeDamage(40);

        Assert.IsTrue(_health.IsDead);
        Assert.AreEqual(0, _slider.value, 1e-4);
    }

    [Test]
    public void TakeDamage_TwoHitsOf40AtHp100_IsAliveWithFifthOfHp()
    {
        _health.TakeDamage(40);
        _health.TakeDamage(40);

        Assert.IsFalse(_health.IsDead);
        Assert.AreEqual(.2f, _slider.value, 1e-4);
    }
}
}
