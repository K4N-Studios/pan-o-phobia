using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    private void Update()
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

    private bool IsPlayerMoving()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }
}
