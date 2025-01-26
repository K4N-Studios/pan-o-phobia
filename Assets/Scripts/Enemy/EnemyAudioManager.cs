using UnityEngine;
using FMODUnity;

public class EnemyAudioManager : MonoBehaviour
{
    public StudioEventEmitter footstepsEmitter;

    [SerializeField] private EnemyMovement _enemyMovement;

    private void Awake()
    {
        if (_enemyMovement == null)
        {
            _enemyMovement = GetComponent<EnemyMovement>();
        }
    }

    private void Update()
    {
        if (IsEnemyMoving() && !footstepsEmitter.IsPlaying())
        {
            footstepsEmitter.Play();
        }
    }

    // if enemy contains enemymovement script it means it will always be moving prolly
    private bool IsEnemyMoving()
    {
        return _enemyMovement.MovementType switch
        {
            EnemyMovement.EMovementType.Patrol => !_enemyMovement.isWaiting,
            EnemyMovement.EMovementType.Chase => true,
            _ => false,
        };
    }
}
