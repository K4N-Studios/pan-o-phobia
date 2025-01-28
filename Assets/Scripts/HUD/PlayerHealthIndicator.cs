using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthIndicator : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;

    [Header("Internals")]
    [SerializeField] private Image _sliderImage;
    [SerializeField] private Image _heartImage;

    [Header("Configuration")]
    [SerializeField] private float _heartBrokenStep = 45.0f;

    [Header("Skins")]
    [SerializeField] private Sprite _spriteFull;
    [SerializeField] private Sprite _spriteBroken;

    private void UpdateIndicator()
    {
        var sliderValue = (float)_playerHealth.CurrentHealth / _playerHealth.MaxHealth;
        _sliderImage.fillAmount = sliderValue;

        if (sliderValue >= _heartBrokenStep / _playerHealth.MaxHealth)
        {
            _heartImage.sprite = _spriteFull;
        }
        else
        {
            _heartImage.sprite = _spriteBroken;
        }
    }

    private void Update()
    {
        UpdateIndicator();
    }
}
