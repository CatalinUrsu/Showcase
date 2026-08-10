using Zenject;
using DG.Tweening;
using FMOD.Studio;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Source.Game.Player
{
public class PlayerEmergence : MonoBehaviour
{
    [SerializeField] Transform _spawnPos;
    [SerializeField] Transform _playPos;
    
    float _playerShowDuration = .5f;
    
    [Inject] protected IAudioService _audioService;

    public void Init(Rigidbody2D rb)
    {
        transform.position = _spawnPos.position;
        rb.position = _spawnPos.position;
    }

    public void Deinit() => _audioService.FlyInstance.stop(STOP_MODE.IMMEDIATE);

    public async UniTask ShowPlayer(Rigidbody2D rb)
    {
        gameObject.SetActive(true);
        _audioService.FlyInstance.start();

        var lerpTime = 0f;
        var showAnimationTween = DOTween.To(() => lerpTime, x => lerpTime = x, 1, _playerShowDuration)
                                        .OnUpdate(() => rb.position = Vector3.Lerp(_spawnPos.position, _playPos.position, lerpTime))
                                        .SetUpdate(UpdateType.Fixed);

        await showAnimationTween.ToUniTask();
    }
}
}