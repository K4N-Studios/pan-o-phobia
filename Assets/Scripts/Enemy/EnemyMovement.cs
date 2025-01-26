using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum EMovementType { Stationary, Patrol, Chase }
    public bool isWaiting = false;
    public float stopChasingTimerDefaultTime = 3f;

    private int _currentPatrolIndex = 0;
    [SerializeField] private float _stopChasingTimerTime = 3f;
    [SerializeField] private bool _stopChasingTimerRunning = false;

    [SerializeField] private EMovementType _movementType = EMovementType.Stationary;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private float _patrolSpeed = 2f;
    [SerializeField] private float _chaseSpeed = 4f;
    [SerializeField] private Transform _player;
    [SerializeField] private bool _isChasing = false;
    [SerializeField] private bool _trynaFollowPlayer = false;

    public EMovementType MovementType => _movementType;

    private void ResetStopChasingTimer()
    {
        _stopChasingTimerTime = stopChasingTimerDefaultTime;
    }

    private void CancelStopChasingTimer()
    {
        ResetStopChasingTimer();
        _stopChasingTimerRunning = false;
    }

    private void Start()
    {
        StartCoroutine(PatrolRoutine());
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            if (_movementType == EMovementType.Patrol)
            {
                if (_isChasing)
                {
                    yield break;
                }
                yield return Patrol();
            }
            else
            {
                yield return null;
            }
        }
    }

    private void SwitchToChaseMode()
    {
        _isChasing = true;
        _movementType = EMovementType.Chase;
    }

    private void SwitchToPatrolMode()
    {
        _isChasing = false;
        _movementType = EMovementType.Patrol;
    }

    // This will make the timer restart when the trigger goes out.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_trynaFollowPlayer && other.gameObject.CompareTag("Player"))
        {
            if (_stopChasingTimerRunning)
            {
                Debug.Log("Stopping running timer");
                CancelStopChasingTimer();
            }

            if (!_isChasing && _movementType == EMovementType.Patrol)
            {
                Debug.Log("Starting to follow player");
                SwitchToChaseMode();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_trynaFollowPlayer && other.gameObject.CompareTag("Player"))
        {
            if (!_stopChasingTimerRunning)
            {
                _stopChasingTimerRunning = true;
                _stopChasingTimerTime = stopChasingTimerDefaultTime;
                Debug.Log("Collision timer has been reset and started");
            }
        }
    }

    private void Update()
    {
        if (_stopChasingTimerRunning == true)
        {
            _stopChasingTimerTime -= Time.deltaTime;
            if (_trynaFollowPlayer && _stopChasingTimerTime <= 0.0f)
            {
                Debug.Log("Lost player after three seconds without no more collider impact");
                CancelStopChasingTimer();
                SwitchToPatrolMode();
            }
        }
    }

    private IEnumerator Patrol()
    {
        if (_patrolPoints.Length == 0 || _isChasing)
        {
            yield break;
        }

        var point = _patrolPoints[_currentPatrolIndex];

        while (Vector2.Distance(transform.position, point.position) > 0.1f)
        {
            if (_isChasing)
            {
                yield break;
            }

            MoveTowards(point.position, _patrolSpeed);
            yield return null;
        }

        isWaiting = true;

        yield return new WaitForSeconds(2f);

        isWaiting = false;
        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;
    }

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
