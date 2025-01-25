using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum MovementType { Stationary, Patrol, Chase }
    [SerializeField] private MovementType _movementType = MovementType.Stationary;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private float _patrolSpeed = 2f;
    [SerializeField] private float _chaseSpeed = 4f;
    [SerializeField] private Transform _player;

    private int _currentPatrolIndex = 0;

    private void Start()
    {
        StartCoroutine(PatrolRoutine());
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            if (_movementType == MovementType.Patrol)
            {
                yield return Patrol();
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator Patrol()
    {
        if (_patrolPoints.Length == 0)
        {
            yield break;
        }

        var point = _patrolPoints[_currentPatrolIndex];

        while (Vector2.Distance(transform.position, point.position) > 0.1f)
        {
            MoveTowards(point.position, _patrolSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;
    }


    public void HandleMovement()
    {
        switch (_movementType)
        {
            case MovementType.Stationary:
                break;
            case MovementType.Patrol:
                Patrol();
                break;
            case MovementType.Chase:
                ChasePlayer();
                break;
        }
    }

   private void ChasePlayer()
    {
        if (_player == null) return;

        MoveTowards(_player.position, _chaseSpeed);
    }

    private void MoveTowards(Vector2 targetPosition, float speed)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
    }
}
