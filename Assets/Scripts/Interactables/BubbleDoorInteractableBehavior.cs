using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BubbleDoorInteractableBehavior : MonoBehaviour
{
    public PlayerStress playerStress;
    public Light2D lightComponent;

    public void TrynaOpen()
    {
        gameObject.SetActive(false);
        playerStress.StartTimer();
        lightComponent.intensity = 0;
    }
}
