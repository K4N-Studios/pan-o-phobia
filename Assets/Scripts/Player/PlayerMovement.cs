using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    private Vector2 _movementInput;
    private Vector2 _lastMovementDirection = Vector2.right;
    [SerializeField] private Transform _flashLight;


    public void HandleMovementInput()
    {
        _movementInput.x = Input.GetAxis("Horizontal");
        _movementInput.y = Input.GetAxis("Vertical");
        
        Move();
        HandleFlashLightMovement();
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
        Vector3 movement = new Vector3(_movementInput.x, _movementInput.y, 0) * _movementSpeed * Time.deltaTime;
        transform.Translate(movement, Space.Self);
    }
}
