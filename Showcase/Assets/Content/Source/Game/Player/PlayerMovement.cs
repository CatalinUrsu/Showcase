using R3;
using Helpers;
using Zenject;
using R3.Triggers;
using UnityEngine;
using Helpers.Audio;
using Helpers.Services;

namespace Source.Game.Player
{
public class PlayerMovement : MonoBehaviour
{
#region Fields

    [SerializeField] Canvas _canvasInputHandler;
    
    [Space]
    [SerializeField] float _maxSpeed = 10;
    [SerializeField] Transform _playPos;
    [SerializeField] ParticleSystem _flyFx;
    
    bool _controlIsEnable;
    bool _isClicked;
    float _maxPosY;
    float _speedVelocity = 30f;

    Camera _camera;
    Vector2 _targetPos;
    Vector2 _moveDirection;
    CompositeDisposable _disposables = new();

    ParticleSystem.MainModule _flyEffectMain;
    ParticleSystem.MinMaxCurve _minMaxCurve;
    
    ICameraService _cameraService;
    IAudioService _audioService;

#endregion

#region Public methods

    [Inject]
    public void Construct(ICameraService cameraService, IAudioService audioService)
    {
        _cameraService = cameraService;
        _audioService = audioService;
    }

    public void Init(Rigidbody2D rb)
    {
        _maxPosY = Screen.height / 3f;
        _camera = _cameraService.GetMainCamera();
        _targetPos = _playPos.position;
        _flyEffectMain = _flyFx.main;
        _minMaxCurve = _flyFx.main.startLifetime;

        SetFlySoundControl(rb);
        SetPlayerControl(rb);
    }

    public void Deinit() => _disposables.Dispose();

    public void PlayMovementFx()
    {
        _flyFx.Play();
        _audioService.FlyInstance.SetParameter(ConstFMOD.FLY_POWER, 0);
    }
    
    public void ToggleControl(bool enable) => _controlIsEnable = enable;

    public void StopMovementFx() => _flyFx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

#endregion

#region Private methods

    void SetPlayerControl(Rigidbody2D rb)
    {
        Observable.EveryUpdate(UnityFrameProvider.FixedUpdate)
                  .Where(_ => _isClicked)
                  .Subscribe(_ => MovePlayer(rb))
                  .AddTo(_disposables);

        _canvasInputHandler.gameObject.AddComponent<ObservablePointerDownTrigger>()
                         .OnPointerDownAsObservable()
                         .Where(_ => _controlIsEnable)
                         .Select(pointer => pointer.position)
                         .Where(position => position.y <= _maxPosY)
                         .Subscribe(OnPointerDown_handler)
                         .AddTo(_disposables);

        _canvasInputHandler.gameObject.AddComponent<ObservablePointerUpTrigger>()
                         .OnPointerUpAsObservable()
                         .Where(_ => _controlIsEnable)
                         .Select(x => x.position)
                         .Subscribe(_ => OnPointerUp_handler())
                         .AddTo(_disposables);

        _canvasInputHandler.gameObject.AddComponent<ObservableDragTrigger>()
                         .OnDragAsObservable()
                         .Where(_ => _controlIsEnable && _isClicked)
                         .Select(pointer => pointer.position)
                         .Subscribe(OnDrag_handler)
                         .AddTo(_disposables);
    }

    void SetFlySoundControl(Rigidbody2D rb)
    {
        Observable.EveryUpdate()
                  .Where(_ => _controlIsEnable)
                  .Subscribe(_ => _audioService.FlyInstance.SetParameter(ConstFMOD.FLY_POWER, rb.linearVelocity.magnitude / 5))
                  .AddTo(_disposables);
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