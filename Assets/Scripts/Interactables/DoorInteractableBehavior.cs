using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorInteractableBehavior : MonoBehaviour
{
    public GameStateManager gameState;
    public UnityEvent OnOpen;
    private FMOD.Studio.EventInstance _sfxOpenInstance;
    [SerializeField] private List<CollectableRegister> _openRequirements = new();

    // NOTE: Teleporter will be the one that will play the close one.
    private void Awake()
    {
        _sfxOpenInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Actions/Door/Open");
    }

    private bool IsLocked()
    {
        var foundRequirements = 0;
        foreach (var requirement in _openRequirements)
        {
            foreach (var collectable in gameState.collectedCollectables)
            {
                if (requirement.Name == collectable.Name && collectable.Amount >= requirement.Amount)
                {
                    foundRequirements++;
                }
            }
        }

        return foundRequirements != _openRequirements.Count;
    }

    private void PlayOpenSFX()
    {
        if (_sfxOpenInstance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE playbackState) == FMOD.RESULT.OK)
        {
            if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
            {
                _sfxOpenInstance.start();
            }
        }
    }

    private void OnDestroy()
    {
        _sfxOpenInstance.release();
    }

    public void OpenDoor()
    {
        if (!IsLocked())
        {
            PlayOpenSFX();
            OnOpen.Invoke();
        }
    }
}
