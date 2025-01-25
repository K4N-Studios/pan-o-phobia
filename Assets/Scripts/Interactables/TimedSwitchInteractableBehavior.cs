using System.Net;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimedSwitchInteractableBehavior : MonoBehaviour
{
    public Light2D globalLight;
    public GameStateManager gameState;

    [SerializeField] private float _lowIntensityLevel = 0.15f;
    [SerializeField] private float _maxIntensityLevel = 1f;
    [SerializeField] private float _timerTime = 10f;
    [SerializeField] private bool _lightIsTurnedOn = false;
    [SerializeField] private bool _timerIsRunning = false;

    public float ShutdownTimerProgress => _timerTime;

    public void ToggleSwitch()
    {
        // ignore request because the main switch takes priority.
        if (gameState.enabledMainLightSwitch)
        {
            if (_timerIsRunning)
            {
                CancelTimer();
            }

            return;
        }

        if (!_lightIsTurnedOn)
        {
            // we'll have to turn the light on and then start a timer which will turn it off after x seconds.
            globalLight.intensity = _maxIntensityLevel;
            _timerIsRunning = true;
        }
        else
        {
            // if it gets here it means that the user wants to cancel the timer because they wants to turns the
            // timer off, so the timer will stop.
            globalLight.intensity = _lowIntensityLevel;
            CancelTimer();
        }
    }

    private void CancelTimer()
    {
        _timerTime = 10f;
        _timerIsRunning = false;
    }

    private void OnTimerEnded()
    {
        if (_lightIsTurnedOn && _timerIsRunning)
        {
            // when the timer gets done, the light will be turned off and the timer will be reset.
            globalLight.intensity = _lowIntensityLevel;
            CancelTimer();
        }
    }

    private void Update()
    {
        // turned on means if the light is turned on or not, we can see if it's by using the intensity of the light 2d component.
        _lightIsTurnedOn = globalLight.intensity == _maxIntensityLevel;

        // we won't update things if the timer is paused or canceled.
        if (_timerIsRunning == false)
        {
            return;
        }

        _timerTime -= Time.deltaTime;

        if (_timerTime <= 0.0f)
        {
            OnTimerEnded();
            _timerTime = 10.0f;
        }
    }
}
