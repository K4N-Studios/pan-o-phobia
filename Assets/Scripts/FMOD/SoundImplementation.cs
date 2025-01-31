public class SoundImplementation
{
    protected FMOD.Studio.EventInstance _soundInstance;

    public SoundImplementation(FMOD.Studio.EventInstance instance)
    {
        _soundInstance = instance;
    }

    public virtual bool IsPlaying()
    {
        if (_soundInstance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE state) == FMOD.RESULT.OK)
        {
            return state == FMOD.Studio.PLAYBACK_STATE.PLAYING;
        }

        return false;
    }

    public virtual void Play()
    {
        _soundInstance.start();
    }

    public virtual void Stop(bool fadeout = false)
    {
        _soundInstance.stop(fadeout switch
        {
            true => FMOD.Studio.STOP_MODE.ALLOWFADEOUT,
            false => FMOD.Studio.STOP_MODE.IMMEDIATE,
        });
    }

    public virtual void Release()
    {
        _soundInstance.release();
    }
}
