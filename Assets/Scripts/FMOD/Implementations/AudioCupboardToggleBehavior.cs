enum CupboardStatus
{
    Opened,
    Closed,
}

// TODO: Attach the actual cupboard object to read.
public class AudioCupboardToggleBehavior : SoundImplementation
{
    private readonly string _fmodStatusParamName = "CupboardStatus";

    public AudioCupboardToggleBehavior(FMOD.Studio.EventInstance instance) : base(instance) { }

    public override void Play()
    {
        var newStatus = CupboardStatus.Opened;
        _soundInstance.setParameterByName(_fmodStatusParamName, (float)newStatus);
        base.Play();
    }
}