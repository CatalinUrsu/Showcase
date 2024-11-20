using DG.Tweening;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Source.Player
{
public class PlayerEmergence : MonoBehaviour
{
    [SerializeField] Transform _spawnPos;
    [SerializeField] Transform _playPos;

    float _playerShowDuration = .5f;

    public void Init(Rigidbody2D rb)
    {
        transform.position = _spawnPos.position;
        rb.position = _spawnPos.position;
    }

    public async UniTask ShowPlayer(Rigidbody2D rb)
    {
        gameObject.SetActive(true);

        var lerpTime = 0f;
        var showAnimationTween = DOTween.To(() => lerpTime, x => lerpTime = x, 1, _playerShowDuration)
                                        .OnUpdate(() => rb.position = Vector3.Lerp(_spawnPos.position, _playPos.position, lerpTime))
                                        .SetUpdate(UpdateType.Fixed);

        await showAnimationTween.ToUniTask();
    }
}
}