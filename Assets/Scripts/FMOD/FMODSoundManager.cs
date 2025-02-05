using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Register new sounds here, also make sure to add the appropiate play behavior after registering a new one.
/// </summary>
[Serializable]
public enum SoundType
{
    PlayerFootsteps,
    FearCrackingWoodEffect,
    GermEnemyDamage,
    ShadowEnemyDamage,
    EnemyFootsteps,
    EnemyDeathSound,
    PlayerFlashlightToggle,
    PlayerHeavyBreathing
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

    // + Events requirements-------------
    //   + Player------------------------
    [SerializeField] private PlayerConfiguration _playerConfiguration;
    [SerializeField] private PlayerManagers _playerManagers;

    [Serializable]
    private class PlayerConfiguration
    {
        public float _heavyBreathingFadeInDuration = 3.0f;
    }

    [Serializable]
    private class PlayerManagers
    {
        public PlayerFlashlightManager _flashlightManager;
    }

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
            // + Player -------------------------------------------------------------------
            SoundType.PlayerFlashlightToggle => new AudioPlayerFlashlightToggleBehavior(instance, flashlightManager: _playerManagers._flashlightManager),
            SoundType.PlayerHeavyBreathing => new AudioPlayFadedBehavior(instance, fadeInDuration: _playerConfiguration._heavyBreathingFadeInDuration),

            // + Ambient sounds -----------------------------------------------------------
            SoundType.FearCrackingWoodEffect => new AudioPlayIfNotRunningBehavior(instance),

            // + Footsteps ----------------------------------------------------------------
            SoundType.PlayerFootsteps => new AudioPlayIfNotRunningBehavior(instance),
            SoundType.EnemyFootsteps => new AudioPlayWithPreStopBehavior(instance),

            // + Enemy damage sounds ------------------------------------------------------
            SoundType.GermEnemyDamage => new AudioPlayIfNotRunningBehavior(instance),
            SoundType.ShadowEnemyDamage => new AudioPlayIfNotRunningBehavior(instance),
            SoundType.EnemyDeathSound => new AudioPlayIfNotRunningBehavior(instance),

            ///////////////////////////////////////////////////////////////////////////////
            _ => throw new AudioImplementationUnavailableException(type),
        };
    }

    public void Play(SoundType sound)
    {
        GetSoundImpl(sound).Play();
    }

    public void PlayOneTime(SoundType sound)
    {
        Play(sound);
        Release(sound);
    }

    public void Stop(SoundType sound, bool fadeout = false)
    {
        GetSoundImpl(sound).Stop(fadeout);
    }

    public bool IsPlaying(SoundType sound)
    {
        return GetSoundImpl(sound).IsPlaying();
    }

    public void Release(SoundType sound)
    {
        GetSoundImpl(sound).Release();
        _instances.Remove(sound);
    }

    /// <summary>
    /// Deallocates the memory of all allocated fmod event instances
    /// </summary>
    private void OnDestroy()
    {
        foreach (var key in _instances.Keys.ToList())
        {
            Release(key);
        }
    }
}
