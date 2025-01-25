using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealt = 100;
    private int _currentHealth;

    public int MaxHealth => _maxHealt;
    public int CurrentHealth => _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealt;
    }

    public void TakeDamage(int ammount)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= ammount;
        }
        Debug.Log("Player took " + ammount + " damage. Current health: " + _currentHealth);
        
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died");
    }

    public void Heal(int ammount)
    {
        _currentHealth += ammount;
        _currentHealth = Mathf.Min(_currentHealth, _maxHealt);
        // _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealt);
        Debug.Log("Player healed " + ammount + " health. Current health: " + _currentHealth);
    }
}
