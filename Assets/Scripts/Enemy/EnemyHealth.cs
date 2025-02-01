using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int CurrentHealth => _currentHealth;

    private int _currentHealth;
    [SerializeField] private int _maxHealth = 30;
    [SerializeField] private SoundType damageSound;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        Debug.Log("Enemy took " + amount + " damage. Current health: " + _currentHealth);
        FMODSoundManager.Instance.Play(damageSound);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy has died");
        Destroy(gameObject);
    }
}
