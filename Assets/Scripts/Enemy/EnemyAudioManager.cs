using UnityEngine;

public class EnemyAudioManager : WithSongManager
{
    [SerializeField] private SoundType _footstepsSound = SoundType.EnemyFootsteps;
    [SerializeField] private EnemyMovement _enemyMovement;

    private FMODUnity.StudioEventEmitter _eventEmitter;

    private void Start()
    {
        var reference = _soundManager.GetSoundEventReference(_footstepsSound);

        if (reference != null)
        {
            _eventEmitter = gameObject.AddComponent<FMODUnity.StudioEventEmitter>();
            _eventEmitter.EventReference = reference.Value;
        }
    }

    private void PlayWith3DAttributes()
    {
        _eventEmitter.EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
        _eventEmitter.Play();
    }

    private void Update()
    {
        if (!_enemyMovement.isWaiting && !_eventEmitter.IsPlaying())
        {
            PlayWith3DAttributes();
        }
    }
}
