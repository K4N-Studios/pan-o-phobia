using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public void ApplyDamage(IDamageable target, int damage)
    {
        target.TakeDamage(damage);
    }
}

public interface IDamageable
{
    void TakeDamage(int amount);
}