using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    private Vector2 _movementInput;

    public void HandleMovementInput()
    {
        _movementInput.x = Input.GetAxis("Horizontal");
        _movementInput.y = Input.GetAxis("Vertical");
        
        Move();  
    }

    private void Move()
    {
        Vector3 movement = new Vector3(_movementInput.x, _movementInput.y, 0) * _movementSpeed * Time.deltaTime;
        transform.Translate(movement, Space.Self);
    }
}
