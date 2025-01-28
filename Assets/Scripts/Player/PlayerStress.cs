using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum TimerMode
{
    Increase,
    Decrease
}

public class PlayerStress : MonoBehaviour
{
    public float defaultStressTimerTime = 35.0f;
    public GameStateManager state;
    public GameObject flashLightObject;
    public PlayerHealth playerHealth;

    [SerializeField] private float _stressTimerTime = 35.0f;
    [SerializeField] private bool _stressTimerRunning = false;
    [SerializeField] private TimerMode _timerMode = TimerMode.Decrease;

    public float StressAmount
    {
        get => 100 - (_stressTimerTime / defaultStressTimerTime * 100);

        set
        {
            _stressTimerTime = (value + (100 - value)) / 100 * defaultStressTimerTime;
        }
    }

    private void RunTimer()
    {
        switch (_timerMode)
        {
            case TimerMode.Increase:
                _stressTimerTime += Time.deltaTime;
                _stressTimerTime = Math.Min(_stressTimerTime, defaultStressTimerTime);
                break;
            case TimerMode.Decrease:
                _stressTimerTime -= Time.deltaTime;
                break;
        }
    }

    // Timer will change its mode from decreasing -> increasing and viceversa depending if the flashlight is on or off, or if the lightning
    // global is turned on or not.
    private void CheckForTimerMode()
    {
        var localLightsTurnedOn = state.lightsStates.FindAll(x => x.IsOn == true).LastOrDefault();

        // check for the existence of some local element turned on.
        if (localLightsTurnedOn != null && localLightsTurnedOn.IsOn == true)
        {
            _timerMode = TimerMode.Increase;
            return;
        }

        _timerMode = state.enabledMainLightSwitch || flashLightObject.activeSelf
            ? TimerMode.Increase
            : TimerMode.Decrease;
    }

    public void StartTimer()
    {
        _stressTimerRunning = true;
    }

    public void ResetTimer()
    {
        _stressTimerTime = defaultStressTimerTime;
    }

    public void CancelTimer()
    {
        ResetTimer();
        _stressTimerRunning = false;
    }

    private void GameOver()
    {
        playerHealth.TakeDamage(playerHealth.MaxHealth);
        CancelTimer();
    }
    
    private void Update()
    {
        if (_stressTimerRunning)
        {
            RunTimer();
            CheckForTimerMode();

            if (_stressTimerTime <= 0.0f)
            {
                GameOver();
            }
        }
    }
}
