using System.Collections;
using System.Linq;
using UnityEngine;

public class FearCrackingWoodAudio : WithSongManager
{
    public GameStateManager gameState;

    [SerializeField] private SoundType _fearCrackingWoodAudio = SoundType.FearCrackingWoodEffect;

    private IEnumerator CheckForLights()
    {
        LocalLightsRegister localLightsTurnedOn = gameState.lightsStates.FindAll(x => x.IsOn == true).LastOrDefault();
        bool shouldStop = (localLightsTurnedOn != null && localLightsTurnedOn.IsOn == true) || gameState.duringGameOverSplash;

        Debug.Log("should stop -> " + (shouldStop ? "yes" : "no"));

        // check for the existence of some local element turned on, using the last turned
        // on as it will be probably the only one that's turned on currently and where the user is at.
        // FIXME: As this could indirectly create a bug where the current light is at the mid position instead
        // of at the last one, we should sort the array in a way that the last one is always at the last spot.
        if (shouldStop)
        {
            _soundManager.Stop(_fearCrackingWoodAudio);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            _soundManager.Play(_fearCrackingWoodAudio);
        }
    }

    private void Update()
    {
        StartCoroutine(CheckForLights());
    }
}
