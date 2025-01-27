using System;
using UnityEngine;

[Flags]
public enum LayerDim
{
    None = 0,
    Layer2 = 1,
    Layer3 = 2,
    Layer4 = 3
}

public class MainBackgroundMusic : MonoBehaviour
{
    public FMODUnity.EventReference fmodEvent;

    private FMOD.Studio.EventInstance _bgInstance;

    [SerializeField] private bool _enableLayer2 = false;
    [SerializeField] private bool _enableLayer3 = false;
    [SerializeField] private bool _enableLayer4 = false;

    private void Start()
    {
        _bgInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        _bgInstance.start();
    }
    
    private void Update()
    {
        _bgInstance.setParameterByName("Dyn Music layer  2", _enableLayer2.GetHashCode());
        _bgInstance.setParameterByName("Dyn Music layer  3", _enableLayer3.GetHashCode());
        _bgInstance.setParameterByName("Dyn Music layer  4", _enableLayer4.GetHashCode());

        if (_bgInstance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE state) == FMOD.RESULT.OK)
        {
            if (state == FMOD.Studio.PLAYBACK_STATE.STOPPED)
            {
                _bgInstance.start();
            }
        }
    }
}
