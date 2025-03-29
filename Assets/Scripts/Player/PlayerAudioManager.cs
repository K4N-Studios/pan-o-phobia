using Unity.Mathematics;
using UnityEngine;
using System.Collections;

public class PlayerAudioManager : WithSongManager
{
    [SerializeField] private PlayerStress _playerStress;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private GameStateManager _state;

    [Header("Songs")]
    [SerializeField] private SoundType _soundFootsteps = SoundType.PlayerFootsteps;
    [SerializeField] private SoundType _soundHeavyBreathing = SoundType.PlayerHeavyBreath;
    [SerializeField] private SoundType _soundHeartbeat = SoundType.PlayerHeartbeat;

    private bool _isHeartbeatFadingOut = false;

    private void CheckFootstepsSound()
    {
        if (IsPlayerMoving())
        {
            _soundManager.Play(_soundFootsteps);
        }
        else
        {
            _soundManager.Stop(_soundFootsteps);
        }
    }

    /// <summary>
    /// We'll have more volume and intensity as the player gets more damage and less
    /// health points. For the calculation of the hearbeat we can use
    /// the next formula:
    /// 
    /// (1 - percentage / max)) ^ 0.6
    /// e.g: (1 - 20 / 100) ^ 0.6 ~= 0.7
    /// </summary>
    private void PlayHeartbeatSound()
    {
        if (_isHeartbeatFadingOut)
        {
            return;
        }

        float percentage = _playerHealth.CurrentHealth;
        float maxHealth = _playerHealth.MaxHealth;
        float heartbeatIntensity = Mathf.Pow(1 - percentage / maxHealth, 0.6f);

        if (!_soundManager.IsPlaying(_soundHeartbeat))
        {
            _soundManager.Play(_soundHeartbeat);
        }

        _soundManager.SetVolume(_soundHeartbeat, heartbeatIntensity);
        _soundManager.SetParameterByName(_soundHeartbeat, "PlayerHeartbeatIntensity", heartbeatIntensity);
    }

    /// <summary>
    /// As tim gets stressed over the time, the volume of the heavy breathing will
    /// increase aswell, a local room light, master room light or the flashlight
    /// should stop tim from stressing too much, that will imply lowing heavy breathing
    /// quite a bit too.
    /// </summary>
    private void PlayHeavyBreathSound()
    {
        float stress = _playerStress.StressAmount;
        float stressedVolume = stress / 100;

        if (!_soundManager.IsPlaying(_soundHeavyBreathing))
        {
            _soundManager.Play(_soundHeavyBreathing);
        }

        _soundManager.SetVolume(_soundHeavyBreathing, stressedVolume);
    }

    /// <summary>
    /// To be called from `Update` on `PlayerController`
    /// </summary>
    public void CheckSounds()
    {
        CheckFootstepsSound();
        PlayHeartbeatSound();
        PlayHeavyBreathSound();
    }

    public void FadeOutHeartbeat(float duration)
    {
        StartCoroutine(FadeOutHeartbeatCoroutine(duration));
    }

    private IEnumerator FadeOutHeartbeatCoroutine(float duration)
    {
        _isHeartbeatFadingOut = true;
        float timer = 0;
        float initialVolume = _soundManager.GetVolume(_soundHeartbeat);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float volume = Mathf.Lerp(initialVolume, 0, timer / duration);
            _soundManager.SetVolume(_soundHeartbeat, volume);
            _soundManager.SetParameterByName(_soundHeartbeat, "PlayerHeartbeatIntensity", volume);
            yield return null;
        }

        _soundManager.Stop(_soundHeartbeat);
    }

    private bool IsPlayerMoving()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }
}
