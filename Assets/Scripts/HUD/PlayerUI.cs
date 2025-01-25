using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private PlayerHealth _playerHealth;

    private void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        _healthBar.value = (float)_playerHealth.CurrentHealth / _playerHealth.MaxHealth;
    }
}
