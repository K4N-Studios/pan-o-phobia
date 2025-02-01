using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField] private SoundType _footstepsSound = SoundType.EnemyFootsteps;
    [SerializeField] private EnemyMovement _enemyMovement;

    private FMODUnity.StudioEventEmitter _eventEmitter;

    private void Awake()
    {
        FMODUnity.EventReference? reference;
        if ((reference = FMODSoundManager.Instance.GetSoundEventReference(_footstepsSound)) == null)
        {
            return;
        }

        _eventEmitter = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
        _eventEmitter.EventReference = reference.Value;
    }

    private void Update()
    {
        if (!_enemyMovement.isWaiting && !_eventEmitter.IsPlaying())
        {
            _eventEmitter.Play();
        }
    }
}
