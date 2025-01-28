using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum EMovementType { Stationary, Patrol, PatrolAndChase, Chase }
    public bool isWaiting = false;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _sprite;

    [SerializeField] private Transform[] _patrolPoints;

    private int _currentPatrolIndex = 0;
    private bool _isMovingForward = true;


    [SerializeField] private EMovementType _movementType = EMovementType.Stationary;
    [SerializeField] private float _patrolSpeed = 2f;
    [SerializeField] private float _patrolWaitTime = 2f;
    [SerializeField] private float _chaseSpeed = 4f;
    [SerializeField] private float _minDistanceToChace = 2.25f;
    [SerializeField] private float _minDistanceToAttack = 1f;
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _playerLayer;

    public EMovementType MovementType => _movementType;


    public void HandleMovement()
    {
        switch (_movementType)
        {
            case EMovementType.Stationary:
                break;
            case EMovementType.Patrol:
                Patrol();
                break;
            case EMovementType.Chase:
                ChasePlayer();
                break;
            case EMovementType.PatrolAndChase:
                PatrolAndChase();
                break;
        }
    }

    private void Patrol()
    {
        if (transform.position != _patrolPoints[_currentPatrolIndex].position)
        {
            MoveTowards(_patrolPoints[_currentPatrolIndex].position, _patrolSpeed);
        } 
        else if (!isWaiting)
        {
            StartCoroutine(StopPatrolling());
        }
    }

    private IEnumerator StopPatrolling()
    {
        isWaiting = true;
        yield return new WaitForSeconds(_patrolWaitTime);

        if (_isMovingForward)
        {
            _currentPatrolIndex++;
            if (_currentPatrolIndex >= _patrolPoints.Length)
            {
                _isMovingForward = false;
                _currentPatrolIndex = _patrolPoints.Length - 2;
            }
        }
        else
        {
            _currentPatrolIndex--;
            if (_currentPatrolIndex < 0)
            {
                _isMovingForward = true;
                _currentPatrolIndex = 1;
            }
        }
        isWaiting = false;
    }

    private void ChasePlayer()
    {
        if (_player == null) return;

        if (Vector2.Distance(transform.position, _player.position) < _minDistanceToAttack)
        {
            isWaiting = true;
            return;
        }
        isWaiting = false;
        MoveTowards(_player.position, _chaseSpeed);
    }

    private void PatrolAndChase()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, _minDistanceToChace, _playerLayer);
        if (playerCollider == null)
        {
            Patrol();
        }
        else
        {
            ChasePlayer();
        }
    }

        private void MoveTowards(Vector2 targetPosition, float speed)
        {   
            float step = speed * Time.deltaTime;
            Vector2 currentPosition = transform.position;
            Vector2 moveTo = Vector2.MoveTowards(currentPosition, targetPosition, step);

            // Calcula la diferencia entre la posición actual y la posición objetivo
            Vector2 movement = moveTo - currentPosition;

            Debug.Log("movement: " + movement);
            Debug.Log(_animator);

            // Actualiza los parámetros del animador
            _animator.SetFloat("MoveX", movement.x * 1000);
            _animator.SetFloat("MoveY", movement.y * 1000);
            _animator.SetBool("IsMove", movement.magnitude > 0 );

            _sprite.flipX = movement.x > 0;


            // Aplica el movimiento
            transform.position = moveTo;
        }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _minDistanceToChace);
    }
}
