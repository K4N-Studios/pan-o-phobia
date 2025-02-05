using UnityEngine;

public class PlayerAudioManager : WithSongManager
{
    [SerializeField] private PlayerStress _playerStress;
    [SerializeField] private float _stressHeavyBreathingPoint = 40.0f;

    [Header("Songs")]
    [SerializeField] private SoundType _soundFootsteps = SoundType.PlayerFootsteps;
    [SerializeField] private SoundType _soundHeavyBreathing = SoundType.PlayerHeavyBreathing;

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

    private void CheckHeavyBreathingSound()
    {
        var stress = _playerStress.StressAmount;
        var isHeavyBreathingPlaying = _soundManager.IsPlaying(_soundHeavyBreathing);

        if (stress >= _stressHeavyBreathingPoint && !isHeavyBreathingPlaying)
        {
            _soundManager.Play(_soundHeavyBreathing);
        }
        else if (stress < _stressHeavyBreathingPoint && isHeavyBreathingPlaying)
        {
            _soundManager.Stop(_soundHeavyBreathing);
        }
    }

    public void CheckSounds()
    {
        CheckFootstepsSound();
        CheckHeavyBreathingSound();
    }

    private bool IsPlayerMoving()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }
}
