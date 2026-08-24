using R3;
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

    [Space]
    [SerializeField] bool _idleMovement;
    [SerializeField, Range(0f, 2f)] float _idleRadius = 0.75f;
    [SerializeField] Vector2 _idleSpeed;

    [Inject] protected IAudioService _audioService;
    readonly CompositeDisposable _idleMoveDisposable = new();

    const float PLAYER_SHOW_DURATION = .5f;

    public void Init(Rigidbody2D rb)
    {
        transform.position = _spawnPos.position;
        rb.position = _spawnPos.position;
    }

    public void Deinit()
    {
        _audioService.FlyInstance.stop(STOP_MODE.IMMEDIATE);
        _idleMoveDisposable?.Clear();
    }

    public async UniTask ShowPlayer(Rigidbody2D rb)
    {
        gameObject.SetActive(true);
        _audioService.FlyInstance.start();

        await DOVirtual.Float(0f, 1f, PLAYER_SHOW_DURATION, MovePlayerToPlayPos)
                       .SetUpdate(UpdateType.Fixed)
                       .ToUniTask();

        if (_idleMovement)
            MoveIdle();
        return;

        void MovePlayerToPlayPos(float lerpValue) => rb.position = Vector3.Lerp(_spawnPos.position, _playPos.position, lerpValue);
    }
    
    public void HidePlayer(Rigidbody2D rb) => rb.position = _spawnPos.position;

    void MoveIdle()
    {
        Vector3 targetPos;
        SetTarget();

        Observable.EveryUpdate()
                  .Subscribe(_=> MovePlayerRandomly())
                  .AddTo(_idleMoveDisposable);
        return;

        void MovePlayerRandomly()
        {
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
                SetTarget();

            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * Random.Range(_idleSpeed.x, _idleSpeed.y));
        }

        void SetTarget() => targetPos = _playPos.position + (Vector3)(Random.insideUnitCircle * _idleRadius);
    }
}
}