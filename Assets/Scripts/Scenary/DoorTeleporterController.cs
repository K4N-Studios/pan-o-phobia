using System.Collections;
using UnityEngine;

public class DoorTeleporterController : MonoBehaviour
{
    public GameObject firstDoor;
    public GameObject secondDoor;
    public GameObject player;

    private FadeController _fadeController;

    private FMOD.Studio.EventInstance _sfxCloseDoor;

    [SerializeField] private float _fadeSeconds = 0.5f;

    private void Awake()
    {
        _fadeController = FindObjectOfType<FadeController>();
        _sfxCloseDoor = FMODUnity.RuntimeManager.CreateInstance("event:/Actions/Door/Close");
    }

    private void TeleportTo(Vector3 position)
    {
        player.transform.position = position;
        
        if (_sfxCloseDoor.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE playbackState) == FMOD.RESULT.OK)
        {
            if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
            {
                _sfxCloseDoor.start();
            }
        }
    }

    private IEnumerator OpenSequence()
    {
        _fadeController.TriggerFade();
        yield return new WaitForSeconds(_fadeSeconds);
        TeleportTo(secondDoor.transform.position);
    }

    public void HandleOnOpen()
    {
        StartCoroutine(OpenSequence());
    }

    private IEnumerator OpenSequenceEnd()
    {
        _fadeController.TriggerFade();
        yield return new WaitForSeconds(_fadeSeconds);
        TeleportTo(firstDoor.transform.position);
    }

    // for second door
    public void HandleOnOpenEnd()
    {
        StartCoroutine(OpenSequenceEnd());
    }

    private void Destroy()
    {
        _sfxCloseDoor.release();
    }
}
