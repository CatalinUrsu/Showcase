using UniRx;
using Helpers;
using UnityEngine;
using DG.Tweening;

namespace Source.Gameplay
{
public class EnemyMovement : MonoBehaviour
{
#region Fields

    [SerializeField] Rigidbody2D _rb;
    [SerializeField] AnimationCurve _hitScaleCurve;

    float _enemyRotation;
    Vector2 _speedDirection;
    Tween _resetMovementTween;
    Tween _hitScaleWTween;
    Transform _spriteTransform;

    CompositeDisposable _disposable = new();
    CompositeDisposable _hitRotationDisposable = new();

#endregion

#region Public methods

    public void Init(Transform spriteTransform) => _spriteTransform = spriteTransform;

    public void Deinit()
    {
        _resetMovementTween.CheckAndEnd(false);
        _hitScaleWTween.CheckAndEnd(false);
        _disposable.Dispose();
        _hitRotationDisposable.Dispose();
    }

    public void Set(Vector2 speedRadius)
    {
        SetMovement(speedRadius);
        SetRotation();
    }

    public void OnHit_handler(Vector2 impulseDirection)
    {
        MoveOnHit(impulseDirection);
        RotateOnHit(-impulseDirection.x);
        ScaleOnHit();
    }

    public void OnDie_handler()
    {
        _resetMovementTween.CheckAndEnd(false);
        _hitScaleWTween.CheckAndEnd(false);
        _disposable.Clear();
        _hitRotationDisposable.Clear();
    }

#endregion

#region Private methods

    void SetMovement(Vector2 speedRadius)
    {
        _spriteTransform.localScale = Vector3.one;

        _speedDirection.x = Random.Range(0, transform.position.x > 0 ? -1f : 1f);
        _speedDirection.y = -Random.Range(speedRadius.x, speedRadius.y);
        _rb.linearVelocity = _speedDirection;
    }

    void SetRotation()
    {
        _disposable.Clear();
        _enemyRotation = Random.Range(-ConstEnemy.IDLE_ROTATION, ConstEnemy.IDLE_ROTATION);
        Observable.EveryFixedUpdate()
                  .Subscribe(_ => _spriteTransform.Rotate(0, 0, _enemyRotation))
                  .AddTo(_disposable);
    }

    void MoveOnHit(Vector2 impulseDirection)
    {
        _rb.AddForce(impulseDirection * 5, ForceMode2D.Impulse);
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Clamp(_rb.linearVelocity.y, -10, -.1f));
        _speedDirection = _rb.linearVelocity * 1.5f;

        _resetMovementTween.CheckAndEnd(false);
        _resetMovementTween = DOTween.To(() => _rb.linearVelocity, endValue => _rb.linearVelocity = endValue, _speedDirection, 2).SetDelay(.5f);
    }

    void RotateOnHit(float impulseX)
    {
        _hitRotationDisposable.Clear();
        _enemyRotation = Mathf.Clamp(_enemyRotation + impulseX * ConstEnemy.HIT_ROT_MULTIPLIER, -ConstEnemy.HIT_ROT_MAX, ConstEnemy.HIT_ROT_MAX);

        var rotateClockwise = _enemyRotation < 0;
        var rotationFinal = rotateClockwise ? Random.Range(-ConstEnemy.IDLE_ROTATION, 0) : Random.Range(0, ConstEnemy.IDLE_ROTATION);
        var rotationDrag = rotateClockwise ? -ConstEnemy.HIT_ROT_DRAG : ConstEnemy.HIT_ROT_DRAG;

        Observable.EveryUpdate()
                  .Subscribe(_ => SlowDownRotation())
                  .AddTo(_hitRotationDisposable);

        void SlowDownRotation()
        {
            _enemyRotation -= rotationDrag;

            if (Mathf.Abs(_enemyRotation) <= Mathf.Abs((int)rotationFinal))
                _hitRotationDisposable.Clear();
        }
    }

    void ScaleOnHit()
    {
        _hitScaleWTween.CheckAndEnd();
        _hitScaleWTween = _spriteTransform.DOPunchScale(ConstEnemy.HIT_SCALE_PUNCH, ConstEnemy.HIT_SCALE_DURATION, 0, 0);
    }

#endregion
}
}