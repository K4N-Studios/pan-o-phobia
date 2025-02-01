using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference _footstepsEvent;
    [SerializeField] private EnemyMovement _enemyMovement;

    private FMODUnity.StudioEventEmitter _eventEmitter;

    private void Awake()
    {
        _eventEmitter = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _eventEmitter.EventReference = _footstepsEvent;
    }

    private void Update()
    {
        if (!_enemyMovement.isWaiting && !_eventEmitter.IsPlaying())
        {
            _eventEmitter.Play();
        }
    }
}
