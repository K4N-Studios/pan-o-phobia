public class FearCrackingWood : SoundImplementation
{
    public FearCrackingWood(FMOD.Studio.EventInstance instance) : base(instance) { }

    public override void Play()
    {
        if (!IsPlaying()) base.Play();
    }

    public override void Stop(bool fadeout = false)
    {
        if (IsPlaying()) base.Stop(fadeout);
    }
}
