/// <summary>
/// This behavior will play the desired song and check if it's running first, if it is
/// it will stop it and re run it again.
/// </summary>
public class AudioPlayWithPreStopBehavior : SoundImplementation
{
    public AudioPlayWithPreStopBehavior(FMOD.Studio.EventInstance instance) : base(instance) { }

    public override void Play()
    {
        if (IsPlaying())
        {
            Stop();
        }

        base.Play();
    }
}
