using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum SoundType
{
    PlayerFootsteps,
    FearCrackingWoodEffect,
    GermEnemyDamage,
    ShadowEnemyDamage,
    EnemyFootsteps,
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

    public FMODUnity.EventReference? GetSoundEventReference(SoundType soundType)
    {
        if (_references.TryGet(soundType, out FMODUnity.EventReference reference))
        {
            return reference;
        }

        return null;
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

    /// <summary>
    /// Lists the behaviors for each sound type.
    /// NOTE: Sound references should be set on the unity inspector.
    /// </summary>
    /// <param name="type">sound type to obtain the behavior</param>
    /// <returns>The appropiate behavior for the given sound type</returns>
    /// <exception cref="AudioImplementationUnavailableException">The given sound type is not found</exception>
    private SoundImplementation GetSoundImpl(SoundType type)
    {
        var instance = GetSoundInstance(type);

        return type switch
        {
            // + Ambient sounds -----------------------------------------------------------
            SoundType.FearCrackingWoodEffect => new AudioPlayIfNotRunningBehavior(instance),

            // + Footsteps ----------------------------------------------------------------
            SoundType.PlayerFootsteps => new AudioPlayIfNotRunningBehavior(instance),
            SoundType.EnemyFootsteps => new AudioPlayWithPreStopBehavior(instance),

            // + Enemy damage sounds ------------------------------------------------------
            SoundType.GermEnemyDamage => new AudioPlayIfNotRunningBehavior(instance),
            SoundType.ShadowEnemyDamage => new AudioPlayIfNotRunningBehavior(instance),

            ///////////////////////////////////////////////////////////////////////////////
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

    /// <summary>
    /// Deallocates the memory of all allocated fmod event instances
    /// </summary>
    private void OnDestroy()
    {
        foreach (var instance in _instances)
        {
            Debug.Log("[FMODSoundManager::OnDestroy] release(): " + instance.Key);
            instance.Value.release();
        }
    }
}
