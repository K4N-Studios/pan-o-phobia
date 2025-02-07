using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Register new sounds here, also make sure to add the appropiate play behavior after registering a new one.
/// NOTE: Make sure to register new sounds on the bottom instead of in the middle
/// because unity will then mess with the order on the inspector when registering
/// references of fmod instances.
/// </summary>
[Serializable]
public enum SoundType
{
    PlayerFootsteps,
    PlayerFlashlightToggle,
    PlayerHeavyBreathing,
    PlayerCollapse,
    FearCrackingWoodEffect,
    GermEnemyDamage,
    ShadowEnemyDamage,
    EnemyFootsteps,
    EnemyDeathSound,
    PlayerDamage,
    LightSwitchToggleSound,
    TimedLightSwitchToggleSound,
    PlayerHeavyBreath,
    PlayerHeartbeat
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

    public SerializableDict<SoundType, FMODUnity.EventReference> EventReferences => _references;

#if UNITY_EDITOR
    [SerializeField] private SerializableDict<SoundType, bool> _activeInstances = new();
#endif

    // Events requirements
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

    // NOTE: Set this before calling `FMODSoundManager.Instance.Play(SoundType.LightSwitchToggleSound)` as it expects a valid
    // `LightSwitchInteractableBehavior` to be able to detect if it should play the on or off state.
    [SerializeField] private LightSwitchInteractableBehavior _inUseLightSwitchInteractable;

    public LightSwitchInteractableBehavior InUseLightSwitchInteractable
    {
        get => _inUseLightSwitchInteractable;
        set => _inUseLightSwitchInteractable = value;
    }

    // NOTE: Set this before calling `FMODSoundManager.Instance.Play(SoundType.TimedLightSwitchToggleSound)` as it expects a valid
    // `TimedSwitchInteractableBehavior` to be able to detect if it should play the on or off state.
    [SerializeField] private TimedSwitchInteractableBehavior _inUseTimedSwitchInteractable;

    public TimedSwitchInteractableBehavior InUseTimedSwitchInteractable
    {
        get => _inUseTimedSwitchInteractable;
        set => _inUseTimedSwitchInteractable = value;
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
#if UNITY_EDITOR
                _activeInstances.Add(soundType, false);
#endif
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
            SoundType.PlayerCollapse => new SoundImplementation(instance),
            SoundType.PlayerDamage => new AudioPlayIfNotRunningBehavior(instance),

            // + Ambient sounds -----------------------------------------------------------
            SoundType.FearCrackingWoodEffect => new AudioPlayIfNotRunningBehavior(instance),

            // + Footsteps ----------------------------------------------------------------
            SoundType.PlayerFootsteps => new AudioPlayIfNotRunningBehavior(instance),
            SoundType.EnemyFootsteps => new AudioPlayWithPreStopBehavior(instance),

            // + Enemy damage sounds ------------------------------------------------------
            SoundType.GermEnemyDamage => new AudioPlayIfNotRunningBehavior(instance),
            SoundType.ShadowEnemyDamage => new AudioPlayIfNotRunningBehavior(instance),
            SoundType.EnemyDeathSound => new AudioPlayIfNotRunningBehavior(instance),

            // + Map artifacts ------------------------------------------------------------
            SoundType.LightSwitchToggleSound => new AudioLightSwitchToggleBehavior(instance, _inUseLightSwitchInteractable),
            SoundType.TimedLightSwitchToggleSound => new AudioTimedLightSwitchToggleBehavior(instance, _inUseTimedSwitchInteractable),

            ///////////////////////////////////////////////////////////////////////////////
            _ => throw new AudioImplementationUnavailableException(type),
        };
    }

    public void Play(SoundType sound)
    {
#if UNITY_EDITOR
        _activeInstances.Update(sound, true);
#endif
        GetSoundImpl(sound).Play();
    }

    public void PlayOneTime(SoundType sound)
    {
        Play(sound);
        Release(sound);
    }

    public void Stop(SoundType sound, bool fadeout = false)
    {
#if UNITY_EDITOR
        _activeInstances.Update(sound, false);
#endif
        GetSoundImpl(sound).Stop(fadeout);
    }

    public bool IsPlaying(SoundType sound)
    {
        var isPlaying = GetSoundImpl(sound).IsPlaying();
#if UNITY_EDITOR
        // update just in case.
        _activeInstances.Update(sound, isPlaying);
#endif
        return isPlaying;
    }

    public void Release(SoundType sound)
    {
        GetSoundImpl(sound).Release();
        _instances.Remove(sound);

#if UNITY_EDITOR
        _activeInstances.Remove(sound);
#endif
    }

    /// <summary>
    /// This function will stop all running fmod sounds and also release them
    /// NOTE: This will be automatically called on `OnDestroy()` For fmodsoundmanager.
    /// </summary>
    /// <param name="fadeout"></param>
    public void ReleaseAndStopAll(bool fadeout = false)
    {
        foreach (var key in _instances.Keys.ToList())
        {
            Stop(key, fadeout: fadeout);
            Release(key);
        }
    }

    private void OnDestroy()
    {
        ReleaseAndStopAll();
    }
}
