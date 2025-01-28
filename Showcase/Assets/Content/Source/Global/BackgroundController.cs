using R3;
using System;
using Helpers;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source
{
public class BackgroundController : MonoBehaviour
{
    [SerializeField] Sprite[] _bgSprites;
    [SerializeField] Material _bgMaterial;

    [Space]
    [SerializeField] bool _runtimeTransition;

    static readonly int MainTexId = Shader.PropertyToID("_FirstTex");
    static readonly int SecondTexId = Shader.PropertyToID("_SecondTex");
    static readonly int Transition = Shader.PropertyToID("_Transition");

    int _mainTexIndex;
    int _secondTexIndex;
    Tween _bgChangeTween;
    TimeSpan _bgChangeInterval = TimeSpan.FromSeconds(50);

    void Awake()
    {
        _mainTexIndex = Random.Range(0, _bgSprites.Length);
        do
        {
            _secondTexIndex = Random.Range(0, _bgSprites.Length);
        } while (_secondTexIndex == _mainTexIndex);

        _bgMaterial.SetTexture(MainTexId, _bgSprites[_mainTexIndex].texture);
        _bgMaterial.SetTexture(SecondTexId, _bgSprites[_secondTexIndex].texture);

        if (!_runtimeTransition) return;

        Observable.Interval(_bgChangeInterval)
                  .Subscribe(_ => StartBgTransition())
                  .AddTo(this);
    }

    void OnDestroy()
    {
        _bgMaterial.SetTexture(MainTexId, _bgSprites[0].texture);
        _bgMaterial.SetTexture(SecondTexId, _bgSprites[0].texture);
        _bgMaterial.SetFloat(Transition, 0);
        _bgChangeTween.CheckAndEnd();
    }

    public void StartBgTransition()
    {
        var currentTransition = _bgMaterial.GetFloat(Transition);
        var transitionForward = currentTransition == 0;
        float targetTransition = transitionForward ? 1 : 0;

        if (transitionForward)
        {
            do
            {
                _secondTexIndex = Random.Range(0, _bgSprites.Length);
            } while (_secondTexIndex == _mainTexIndex);

            _bgMaterial.SetTexture(SecondTexId, _bgSprites[_secondTexIndex].texture);
        }
        else
        {
            do
            {
                _mainTexIndex = Random.Range(0, _bgSprites.Length);
            } while (_secondTexIndex == _mainTexIndex);

            _bgMaterial.SetTexture(MainTexId, _bgSprites[_mainTexIndex].texture);
        }

        _bgChangeTween = DOTween.To(x => currentTransition = x, currentTransition, targetTransition, 4f)
                                .SetEase(Ease.Linear)
                                .OnUpdate(() => _bgMaterial.SetFloat(Transition, currentTransition));
    }
}
}