using System.Linq;
using UnityEngine;

public class FearCrackingWoodAudio : MonoBehaviour
{
    public GameStateManager gameState;
    public FMODUnity.EventReference crackingWoodEvent;

    [SerializeField] private FMOD.Studio.EventInstance _crackingWoodInstance;

    private void Start()
    {
        _crackingWoodInstance = FMODUnity.RuntimeManager.CreateInstance(crackingWoodEvent);
    }

    private void StartCrackingWoodSFX()
    {
        if (_crackingWoodInstance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE state) == FMOD.RESULT.OK)
        {
            if (state == FMOD.Studio.PLAYBACK_STATE.STOPPED)
            {
                _crackingWoodInstance.start();
            }
        }
    }

    private void StopCrackingWoodSFX(bool fadeout = true)
    {
        if (_crackingWoodInstance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE state) == FMOD.RESULT.OK)
        {
            if (state == FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                _crackingWoodInstance.stop(fadeout switch
                {
                    true => FMOD.Studio.STOP_MODE.ALLOWFADEOUT,
                    false => FMOD.Studio.STOP_MODE.IMMEDIATE,
                });
            }
        }
    }

    private void CheckForLights()
    {
        var localLightsTurnedOn = gameState.lightsStates.FindAll(x => x.IsOn == true).LastOrDefault();

        // check for the existence of some local element turned on, using the last turned on as it will be probably the only one
        // that's turned on currently and where the user is at.
        if ((localLightsTurnedOn != null && localLightsTurnedOn.IsOn == true) || gameState.duringGameOverSplash)
        {
            Debug.Log("[FearCrackingWood]: stopping sound");
            StopCrackingWoodSFX();
        }
        else
        {
            Debug.Log("[FearCrackingWood]: starting sound");
            StartCrackingWoodSFX();
        }
    }

    private void Update()
    {
        CheckForLights();
    }

    private void OnDestroy()
    {
        _crackingWoodInstance.release();
    }
}
