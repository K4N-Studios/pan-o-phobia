using System.Linq;
using UnityEngine;

public class FearCrackingWoodAudio : MonoBehaviour
{
    public GameStateManager gameState;

    private void CheckForLights()
    {
        LocalLightsRegister localLightsTurnedOn = gameState.lightsStates.FindAll(x => x.IsOn == true).LastOrDefault();
        bool shouldStop = (localLightsTurnedOn != null && localLightsTurnedOn.IsOn == true) || gameState.duringGameOverSplash;

        // check for the existence of some local element turned on, using the last turned
        // on as it will be probably the only one that's turned on currently and where the user is at.
        // FIXME: As this could indirectly create a bug where the current light is at the mid position instead
        // of at the last one, we should sort the array in a way that the last one is always at the last spot.
        if (shouldStop)
        {
            FMODSoundManager.Instance.Stop(SoundType.FearCrackingWoodEffect, fadeout: true);
        }
        else
        {
            FMODSoundManager.Instance.Play(SoundType.FearCrackingWoodEffect);
        }
    }

    private void Update()
    {
        CheckForLights();
    }
}
