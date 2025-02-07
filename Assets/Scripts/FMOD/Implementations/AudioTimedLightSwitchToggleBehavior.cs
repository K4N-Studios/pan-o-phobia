public class AudioTimedLightSwitchToggleBehavior : SoundImplementation
{
    private TimedSwitchInteractableBehavior _lightSwitchInteractable;
    private readonly string _fmodStatusParamName = "SwitchToggleStatus";

    public AudioTimedLightSwitchToggleBehavior(
        FMOD.Studio.EventInstance instance,
        TimedSwitchInteractableBehavior lightSwitchInteractable
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
