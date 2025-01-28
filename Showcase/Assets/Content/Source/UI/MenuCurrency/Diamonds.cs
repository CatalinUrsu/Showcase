using R3;
using Helpers;
using IdleNumbers;
using DG.Tweening;
using UnityEngine;

namespace Source.UI
{
public class Diamonds : Currency
{
    IdleNumber _currentDiamondsCount;
    Sequence _sequence;
    const int _increasingIterations = 5;
    const float _animationDuration = .25f;

    void Awake()
    {
        _currentDiamondsCount = Session.SessionManager.Current.Progress.Diamonds.Value;
        Session.SessionManager.Current.Progress.Diamonds.Subscribe(UpdateDiamonsCount).AddTo(gameObject);
    }

    void OnDestroy() => _sequence.CheckAndEnd();

    void UpdateDiamonsCount(IdleNumber diamonsCount)
    {
        if (diamonsCount.Value > _currentDiamondsCount.Value)
            PlayDiamondsIncreaseAnimation(diamonsCount);
        else
        {
            _currentDiamondsCount = diamonsCount;
            UpdateCurrencyText(_currentDiamondsCount);
        }
    }

    void PlayDiamondsIncreaseAnimation(IdleNumber diamonsCount)
    {
        var diamondsPerIteration = (diamonsCount.Value - _currentDiamondsCount.Value) / _increasingIterations;
        _sequence = DOTween.Sequence().Pause();

        for (int i = 0; i < _increasingIterations; i++)
        {
            var index = i;
            _sequence.Insert(_animationDuration * i / 2, _txtCurrency.rectTransform.DOScale(Vector3.one * 1.2f, _animationDuration)
                                                                    .From(Vector3.one)
                                                                    .OnStart(() =>
                                                                    {
                                                                        _currentDiamondsCount = index == _increasingIterations - 1 ? diamonsCount : _currentDiamondsCount + diamondsPerIteration;
                                                                        UpdateCurrencyText(_currentDiamondsCount);
                                                                    }));
        }

        _sequence.Play();
    }
}
}