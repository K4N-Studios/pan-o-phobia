using UnityEngine;
using UnityEngine.UI;

public class PlayerStressIndicator : MonoBehaviour
{
    [SerializeField] private PlayerStress _playerStress;

    [Header("Internals")]
    [SerializeField] private Image _sliderImage;
    [SerializeField] private Image _faceImage;

    [Header("Configuration")]
    [SerializeField] private float _secondFaceStep = 45.0f;

    [Header("Skins")]
    [SerializeField] private Sprite _spriteFull;
    [SerializeField] private Sprite _spriteDepressed;

    private void Awake()
    {
        if (_sliderImage == null)
        {
            _sliderImage = GetComponent<Image>();
        }

        if (_faceImage == null)
        {
            _faceImage = GetComponentInChildren<Image>();
        }
    }

    private void UpdateIndicator()
    {
        var sliderValue = _playerStress.StressAmount / 100;
        _sliderImage.fillAmount = sliderValue;

        if (sliderValue >= _secondFaceStep / 100)
        {
            _faceImage.sprite = _spriteDepressed;
        }
        else
        {
            _faceImage.sprite = _spriteFull;
        }
    }

    private void Update()
    {
        UpdateIndicator();
    }
}
