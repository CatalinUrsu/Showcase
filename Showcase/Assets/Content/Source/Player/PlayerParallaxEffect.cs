using R3;
using Zenject;
using UnityEngine;
using Helpers.Services;

namespace Source.Player
{
public class PlayerParallaxEffect : MonoBehaviour
{
    [SerializeField] float _parallaxX = 0.1f;
    [SerializeField] float _parallaxY = 0.1f;

    Transform _cameraTransform;
    Vector3 _lastPlayerPosition;
    Vector3 _cameraInitPos;
    readonly CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    public void Construct(IServiceCamera _serviceCamera)
    {
        _cameraTransform = _serviceCamera.GetMainCamera().transform;
        _cameraInitPos = _cameraTransform.position;
    }

    public void EnableParalax()
    {
        _lastPlayerPosition = transform.position;

        Observable.EveryUpdate()
                  .Subscribe(_ => UpdateParallaxEffect())
                  .AddTo(_disposables);
    }
    
    public void Deinit()
    {
        _disposables.Dispose();
        _cameraTransform.position = _cameraInitPos;
    }

    public void DisableParallax() => _disposables.Clear();

    void UpdateParallaxEffect()
    {
        Vector3 deltaMovement = transform.position - _lastPlayerPosition;

        _cameraTransform.position += new Vector3(deltaMovement.x * _parallaxX, deltaMovement.y * _parallaxY, 0);
        _lastPlayerPosition = transform.position;
    }
}
}