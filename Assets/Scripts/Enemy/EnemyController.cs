using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyMovement _enemyMovement;
    [SerializeField] private EnemyAttack _enemyAttack;

    private void Update()
    {
        _enemyMovement.HandleMovement();
        _enemyAttack.HandleAttack();
    }
}
