using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimedSwitchInteractableBehavior : WithSongManager
{
    [SerializeField] private Light2D _light;
    [SerializeField] private GameStateManager _gameState;

    [SerializeField] private float _lowIntensityLevel = 0.15f;
    [SerializeField] private float _maxIntensityLevel = 1f;

    [SerializeField] private float _timerDefaultTime = 10f;
    [SerializeField] private float _timerTime = 10f;
    [SerializeField] private bool _lightIsTurnedOn = false;
    [SerializeField] private bool _timerIsRunning = false;

    [SerializeField] private SoundType _toggleSound = SoundType.TimedLightSwitchToggleSound;

    public float ShutdownTimerProgress => _timerTime;
    public float DefaultTimerTime => _timerDefaultTime;
    public bool TimerIsRunning => _timerIsRunning;
    public bool IsOn => _lightIsTurnedOn;

    private void SetCachedValue(bool newValue)
    {
        _gameState.RegisterLightState(gameObject.name, newValue);
    }

    private void TurnLightOn()
    {
        _light.intensity = _maxIntensityLevel;
        _soundManager.InUseTimedSwitchInteractable = this;
        _soundManager.Play(_toggleSound);
    }

    private void TurnLightOff()
    {
        _light.intensity = _lowIntensityLevel;
        _soundManager.InUseTimedSwitchInteractable = this;
        _soundManager.Play(_toggleSound);
    }

    public void ToggleSwitch()
    {
        // ignore request because the main switch takes priority.
        if (_gameState.enabledMainLightSwitch)
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
            TurnLightOn();
            SetCachedValue(true);
            _timerIsRunning = true;
        }
        else
        {
            // if it gets here it means that the user wants to cancel the timer because they wants to turns the
            // timer off, so the timer will stop.
            TurnLightOff();
            CancelTimer();
            SetCachedValue(false);
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
            TurnLightOff();
            CancelTimer();
            SetCachedValue(false);
        }
    }

    private void UpdateTimerState()
    {
        if (_timerIsRunning == false)
        {
            return;
        }

        _timerTime -= Time.deltaTime;

        if (_timerTime <= 0.0f)
        {
            OnTimerEnded();
            _timerTime = _timerDefaultTime;
        }
    }

    private void Update()
    {
        // turned on means if the light is at its maximum possible intensity,
        // we can see if it that's the case by using the intensity of the light 2d component.
        _lightIsTurnedOn = _light.intensity == _maxIntensityLevel;

        // we won't update things if the timer is paused or canceled.
        UpdateTimerState();
    }
}
