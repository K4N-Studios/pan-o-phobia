using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum SoundType
{
    PlayerFootsteps,
    FearCrackingWoodEffect
}

[Serializable]
class AudioImplementationUnavailableException : Exception
{
    public AudioImplementationUnavailableException() { }

    public AudioImplementationUnavailableException(SoundType sound)
        : base("Unavailable audio implementation found for sound type: " + sound) { }
}

public class FMODSoundManager : Singleton<FMODSoundManager>
{
    private Dictionary<SoundType, FMOD.Studio.EventInstance> _instances = new();
    [SerializeField] private SerializableDict<SoundType, FMODUnity.EventReference> _references = new();

    protected override void Awake()
    {
        base.Awake();
    }

    private FMOD.Studio.EventInstance GetSoundInstance(SoundType soundType)
    {
        if (!_instances.ContainsKey(soundType))
        {
            if (_references.TryGet(soundType, out FMODUnity.EventReference reference))
            {
                _instances.Add(soundType, FMODUnity.RuntimeManager.CreateInstance(reference));
            }
        }

        return _instances[soundType];
    }

    private SoundImplementation GetSoundImpl(SoundType type)
    {
        var instance = GetSoundInstance(type);

        return type switch
        {
            SoundType.FearCrackingWoodEffect => new FearCrackingWood(instance),
            SoundType.PlayerFootsteps => new AudioPlayerFootsteps(instance),
            _ => throw new AudioImplementationUnavailableException(type),
        };
    }

    public void Play(SoundType sound)
    {
        GetSoundImpl(sound).Play();
    }

    public void Stop(SoundType sound, bool fadeout = false)
    {
        GetSoundImpl(sound).Stop(fadeout);
    }

    public void Release(SoundType sound)
    {
        GetSoundImpl(sound).Release();
    }
}
