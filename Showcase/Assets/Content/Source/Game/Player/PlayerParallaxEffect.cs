using Helpers.Services;
using R3;
using UnityEngine;
using Zenject;

namespace Source.Game.Player
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
    public void Construct(ICameraService cameraService)
    {
        _cameraTransform = cameraService.GetMainCamera().transform;
        _cameraInitPos = _cameraTransform.position;
    }

    public void Enable()
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