using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum MovementType { Stationary, Patrol, Chase }
    [SerializeField] private MovementType _movementType = MovementType.Stationary;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private float _movementSpeed = 2f;
    [SerializeField] private Transform _player;

    private int _currentPatrolIndex = 0;

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

    private void Patrol()
    {
        if (_patrolPoints.Length == 0) return;

        Transform targetPosition = _patrolPoints[_currentPatrolIndex];
        MoveTowards(targetPosition.position);

        if (Vector2.Distance(transform.position, targetPosition.position) < 0.1f)
        {
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;
        }
    }

    private void ChasePlayer()
    {
        if (_player == null) return;
        MoveTowards(_player.position);
    }

    private void MoveTowards(Vector2 target)
    {
        Vector3 direction = target - (Vector2)transform.position.normalized;
        transform.position += direction * _movementSpeed * Time.deltaTime;

        // transform.Translate(movement * _movementSpeed * Time.deltaTime, Space.World);
    }
}
