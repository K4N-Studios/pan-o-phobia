enum LightSwitchToggleStatus
{
    On,
    Off
}

public class AudioLightSwitchToggleBehavior : SoundImplementation
{
    private LightSwitchInteractableBehavior _lightSwitchInteractable;
    private readonly string _fmodStatusParamName = "SwitchToggleStatus";

    public AudioLightSwitchToggleBehavior(
        FMOD.Studio.EventInstance instance,
        LightSwitchInteractableBehavior lightSwitchInteractable
    ) : base(instance)
    {
        _lightSwitchInteractable = lightSwitchInteractable;
    }

    public override void Play()
    {
        var newStatusValue = _lightSwitchInteractable.IsOn ? LightSwitchToggleStatus.On : LightSwitchToggleStatus.Off;
        _soundInstance.setParameterByName(_fmodStatusParamName, (float)newStatusValue);
        base.Play();
    }
}
