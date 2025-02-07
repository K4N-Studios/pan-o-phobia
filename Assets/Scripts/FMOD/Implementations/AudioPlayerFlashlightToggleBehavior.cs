enum FlashlightToggleStatus
{
    On,
    Off
}

public class AudioPlayerFlashlightToggleBehavior : SoundImplementation
{
    private PlayerFlashlightManager _flashlightManager;
    private readonly string _toggleStatusParamName = "FlashlightToggleStatus";

    public AudioPlayerFlashlightToggleBehavior(
        FMOD.Studio.EventInstance instance,
        PlayerFlashlightManager flashlightManager
    ) : base(instance)
    {
        _flashlightManager = flashlightManager;
    }

    public override void Play()
    {
        var toggleStatus = _flashlightManager.FlashlightIsActive ? FlashlightToggleStatus.On : FlashlightToggleStatus.Off;
        _soundInstance.setParameterByName(_toggleStatusParamName, (float)toggleStatus);
        base.Play();
    }
}
