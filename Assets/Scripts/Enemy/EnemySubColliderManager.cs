using UnityEngine;

public class EnemySubColliderManager : MonoBehaviour
{
    [SerializeField] private bool _isColliding = false;
    public bool IsColliding => _isColliding;

    private void OnTriggerEnter2D(Collider2D _other)
    {
        _isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        _isColliding = false;
    }
}
