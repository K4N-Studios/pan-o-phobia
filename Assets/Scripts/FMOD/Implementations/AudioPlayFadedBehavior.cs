using System;
using System.Collections;
using UnityEngine;

public class AudioPlayFadedBehavior : SoundImplementation
{
    private readonly float _fadeDuration = 3f;
    private readonly float _initialVolume = 0f;
    private readonly float _targetVolume = 1f;

    // ikr 😭
    private readonly float _fmodReadyDelay = 1f;

    public AudioPlayFadedBehavior(FMOD.Studio.EventInstance soundInstance, float fadeInDuration) : base(soundInstance)
    {
        _fadeDuration = fadeInDuration;
    }

    private IEnumerator FadeIn()
    {
        _soundInstance.start();
        SetVolume(0f);

        // fmod being dumb idk 💀
        yield return new WaitForSeconds(_fmodReadyDelay);

        var elapsedTime = 0.0f;
        while (elapsedTime < _fadeDuration && IsPlaying())
        {
            elapsedTime += Time.deltaTime;
            SetVolume(Mathf.Lerp(_initialVolume, _targetVolume, elapsedTime / _fadeDuration));
            yield return null;
        }

        if (IsPlaying())
        {
            SetVolume(_targetVolume);
        }
    }

    public override void Play()
    {
        CoroutineRunner.Instance.RunCoroutine(FadeIn());
    }
}
