using R3;
using Zenject;
using Helpers;
using UnityEngine;
using FMOD.Studio;
using R3.Triggers;
using Helpers.Audio;
using Helpers.Services;

namespace Source.Player
{
public class PlayerMovement : MonoBehaviour
{
#region Fields

    [SerializeField] float _maxSpeed = 10;
    [SerializeField] Transform _playPos;
    [SerializeField] ParticleSystem _flyEffect;

    public bool ControllIsEnable { get; set; }

    bool _isClicked;
    float _maxPosY;
    float _speedVelocity = 30f;

    Vector2 _targetPos;
    Vector2 _moveDirection;
    Camera _camera;
    EventInstance _flySoundInstance;

    [Inject] IServiceCamera _serviceCamera;
    ParticleSystem.MainModule _flyEffectMain;
    ParticleSystem.MinMaxCurve _minMaxCurve;

#endregion

#region Public methods

    public void Init(Rigidbody2D rb, EventInstance flyEventInstance, GameObject playerInputHanler)
    {
        _maxPosY = Screen.height / 3f;
        _camera = _serviceCamera.GetMainCamera();
        _flyEffectMain = _flyEffect.main;
        _minMaxCurve = _flyEffect.main.startLifetime;

        SetFlySoundControll(rb, flyEventInstance);
        SetPlayerControll(rb, playerInputHanler);
    }

    public void SetOnSpawn()
    {
        _targetPos = _playPos.position;
        _flyEffect.Play();
    }
    
    public void SetOnDespawn()
    {
        _flyEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

#endregion

#region Private methods

    void SetPlayerControll(Rigidbody2D rb, GameObject playerInputHanler)
    {
        Observable.EveryUpdate(UnityFrameProvider.FixedUpdate)
                  .Where(_ => _isClicked)
                  .Subscribe(_ => MovePlayer(rb))
                  .AddTo(gameObject);

        playerInputHanler.AddComponent<ObservablePointerDownTrigger>()
                         .OnPointerDownAsObservable()
                         .Where(_ => ControllIsEnable)
                         .Select(pointer => pointer.position)
                         .Where(position => position.y <= _maxPosY)
                         .Subscribe(OnPointerDown_handler);

        playerInputHanler.AddComponent<ObservablePointerUpTrigger>()
                         .OnPointerUpAsObservable()
                         .Where(_ => ControllIsEnable)
                         .Select(x => x.position)
                         .Subscribe(_ => OnPointerUp_handler());

        playerInputHanler.AddComponent<ObservableDragTrigger>()
                         .OnDragAsObservable()
                         .Where(_ => ControllIsEnable && _isClicked)
                         .Select(pointer => pointer.position)
                         .Subscribe(OnDrag_handler);
    }

    void SetFlySoundControll(Rigidbody2D rb, EventInstance flyEventInstance)
    {
        _flySoundInstance = flyEventInstance;
        Observable.EveryUpdate()
                  .Where(_ => ControllIsEnable)
                  .Subscribe(_ => _flySoundInstance.SetParameter(ConstFMOD.FLY_POWER, rb.linearVelocity.magnitude / 5))
                  .AddTo(gameObject);
    }

    void OnPointerDown_handler(Vector2 inputPos)
    {
        SetTargetPosition(inputPos);
        _isClicked = true;
    }

    void OnPointerUp_handler() => _isClicked = false;

    void OnDrag_handler(Vector2 inputPos) => SetTargetPosition(inputPos);

    void SetTargetPosition(Vector2 inputPos)
    {
        inputPos.y = Mathf.Clamp(inputPos.y, 0, _maxPosY);
        _targetPos = _camera.ScreenToWorldPoint(inputPos);
    }

    void MovePlayer(Rigidbody2D rb)
    {
        _moveDirection = _targetPos - (Vector2)transform.position;

        if (_moveDirection.magnitude < .1f)
        {
            rb.linearVelocity = Vector2.zero;
            SetFlyEffectLifeTime(rb.linearVelocity.y);
            return;
        }

        rb.AddForce(_moveDirection * _speedVelocity);
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, _maxSpeed);
        SetFlyEffectLifeTime(rb.linearVelocity.y);
    }

    void SetFlyEffectLifeTime(float speed)
    {
        _minMaxCurve.constant = speed.Remap(-5, 1, .1f, .3f);
        _flyEffectMain.startLifetime = _minMaxCurve;
    }

#endregion
}
}