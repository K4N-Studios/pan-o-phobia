using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private LayerMask _playerLayer;

    public void HandleAttack()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, _attackRange, _playerLayer);
        if (player != null && player.TryGetComponent<IDamageable>(out IDamageable target))
        {
            Debug.Log("Player in range and taking damage");
            if (player.GetComponent<PlayerHealth>().CurrentHealth <= 0)
            {
                return;
            }
            target.TakeDamage(_damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
