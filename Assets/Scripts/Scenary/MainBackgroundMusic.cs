using UnityEngine;

public class MainBackgroundMusic : MonoBehaviour
{
    private FMOD.Studio.EventInstance instance;

    public FMODUnity.EventReference fmodEvent;

    private void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
    }
    
    private void Update()
    {
        instance.setParameterByName("Dyn Music layer  2", 1);
        instance.setParameterByName("Dyn Music layer  3", 1);
        instance.setParameterByName("Dyn Music layer  4", 1);
    }
}
