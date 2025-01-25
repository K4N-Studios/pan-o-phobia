using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitchInteractableBehavior : MonoBehaviour
{
    public Light2D globalLight;
    public GameStateManager gameState;

    [SerializeField] private float _lowIntensityLevel = 0.15f;
    [SerializeField] private float _maxIntensityLevel = 1f;

    public void ToggleSwitch()
    {
        var newIntensity = 0.0f;

        if (globalLight.intensity == _lowIntensityLevel) newIntensity = _maxIntensityLevel;
        else if (globalLight.intensity == _maxIntensityLevel) newIntensity = _lowIntensityLevel;

        globalLight.intensity = newIntensity;
        gameState.enabledMainLightSwitch = globalLight.intensity == _maxIntensityLevel;
    }
}
