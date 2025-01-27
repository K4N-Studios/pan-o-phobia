using UnityEngine;
using FMODUnity;

public class PlayerAudioManager : MonoBehaviour
{
    public StudioEventEmitter footstepsEmitter;

    private void Update()
    {
        if (IsPlayerMoving() && !footstepsEmitter.IsPlaying())
        {
            footstepsEmitter.Play();
        }
        else if (!IsPlayerMoving() && footstepsEmitter.IsPlaying())
        {
            footstepsEmitter.Stop();
        }
    }

    private bool IsPlayerMoving()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }
}
