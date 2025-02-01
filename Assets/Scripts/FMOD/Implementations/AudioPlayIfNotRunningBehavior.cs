/// <summary>
/// Given a `FMOD.Studio.EventInstance`, this sound implementation will only play/stop it if it's [not] running
/// when asked to do one of those actions.
/// </summary>
public class AudioPlayIfNotRunningBehavior : SoundImplementation
{
    public AudioPlayIfNotRunningBehavior(FMOD.Studio.EventInstance instance) : base(instance) { }

    public override void Play()
    {
        if (!IsPlaying()) base.Play();
    }

    public override void Stop(bool fadeout = false)
    {
        if (IsPlaying()) base.Stop(fadeout);
    }
}
