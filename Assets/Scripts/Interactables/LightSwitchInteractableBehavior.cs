using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitchInteractableBehavior : WithSongManager
{
    [SerializeField] private Light2D _light;
    [SerializeField] private GameStateManager _gameState;
    [SerializeField] private SoundType _lightSwitchToggleSound = SoundType.LightSwitchToggleSound;

    [SerializeField] private readonly float _lowIntensityLevel = 0.15f;
    [SerializeField] private readonly float _maxIntensityLevel = 1f;

    public bool IsOn => _light.intensity == _maxIntensityLevel;

    public void ToggleSwitch()
    {
        var newIntensity = _light.intensity == _maxIntensityLevel ? _lowIntensityLevel : _maxIntensityLevel;
        _light.intensity = newIntensity;
        _gameState.enabledMainLightSwitch = newIntensity == _maxIntensityLevel;
        _soundManager.InUseLightSwitchInteractable = this;
        _soundManager.Play(_lightSwitchToggleSound);
    }
}
