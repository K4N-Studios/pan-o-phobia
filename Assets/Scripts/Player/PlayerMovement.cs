using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    private Vector2 _movementInput;
    private Vector2 _lastMovementDirection = Vector2.down;
    [SerializeField] private Transform _flashLight;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameStateManager _state;

    [SerializeField] private bool restrictDiagonalMovement = true;

    private bool _isMoving = false;
    private float _lastXInputTime;
    private float _lastYInputTime;
    private float _prevInputX;
    private float _prevInputY;

    public void HandleMovementInput()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (_state.duringGameOverSplash || _state.blockControlsRequest)
        {
            _movementInput = Vector2.zero;
            return;
        }

        UpdateInputTiming(ref inputX, ref inputY);

        if (restrictDiagonalMovement)
        {
            HandlePrioritizedMovement(inputX, inputY);
        }
        else
        {
            _movementInput = new Vector2(inputX, inputY).normalized;
        }

        Move();
        HandleFlashLightMovement();
    }

    private void UpdateInputTiming(ref float inputX, ref float inputY)
    {
        if (inputX != 0 && _prevInputX == 0) _lastXInputTime = Time.time;
        if (inputY != 0 && _prevInputY == 0) _lastYInputTime = Time.time;

        _prevInputX = inputX;
        _prevInputY = inputY;
    }

    private void HandlePrioritizedMovement(float inputX, float inputY)
    {
        if (inputX == 0 || inputY == 0)
        {
            _movementInput = new Vector2(inputX, inputY);
            return;
        }

        _movementInput = _lastXInputTime > _lastYInputTime
            ? new Vector2(inputX, 0)
            : new Vector2(0, inputY);
    }

    public void HandleFlashLightMovement()
    {
        if (_movementInput != Vector2.zero)
        {
            _lastMovementDirection = _movementInput.normalized;
        }

        if (_flashLight != null)
        {
            float angle = Mathf.Atan2(_lastMovementDirection.y, _lastMovementDirection.x) * Mathf.Rad2Deg;
            _flashLight.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }

    private void Move()
    {
        _isMoving = _movementInput != Vector2.zero;
        _sprite.flipX = _movementInput.x < 0;

        Vector3 movement = new Vector3(_movementInput.x, _movementInput.y, 0) * _movementSpeed * Time.deltaTime;
        _animator.SetFloat("MoveX", _movementInput.x);
        _animator.SetFloat("MoveY", _movementInput.y);
        _animator.SetFloat("Speed", _isMoving ? 1 : 0);

        transform.Translate(movement, Space.Self);
    }
}
