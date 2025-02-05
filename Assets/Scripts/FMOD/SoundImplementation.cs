using UnityEngine;

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

    public virtual void SetVolume(float newVolume)
    {
        Debug.Log("SetVolume(" + newVolume + ")");
        FMOD.RESULT err;
        if ((err = _soundInstance.setVolume(newVolume)) != FMOD.RESULT.OK)
        {
            Debug.LogError("Unable to set volume of sound " + _soundInstance + " to " + newVolume + ": " + err);
        }
    }

    public virtual float GetVolume()
    {
        if (_soundInstance.getVolume(out float volume) == FMOD.RESULT.OK)
        {
            return volume;
        }

        return default(float);
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
