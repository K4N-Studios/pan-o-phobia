using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private PlayerStress _playerStress;
    [SerializeField] private float _stressHeavyBreathingPoint = 40.0f;

    private void CheckFootstepsSound()
    {
        if (IsPlayerMoving())
        {
            FMODSoundManager.Instance.Play(SoundType.PlayerFootsteps);
        }
        else
        {
            FMODSoundManager.Instance.Stop(SoundType.PlayerFootsteps);
        }
    }

    private void CheckHeavyBreathingSound()
    {
        var stress = _playerStress.StressAmount;
        var isSongPlaying = FMODSoundManager.Instance.IsPlaying(SoundType.PlayerHeavyBreathing);
        if (stress >= _stressHeavyBreathingPoint && !isSongPlaying)
        {
            FMODSoundManager.Instance.Play(SoundType.PlayerHeavyBreathing);
        }
        else if (stress < _stressHeavyBreathingPoint && isSongPlaying)
        {
            FMODSoundManager.Instance.Stop(SoundType.PlayerHeavyBreathing, fadeout: true);
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
