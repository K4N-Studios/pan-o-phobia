using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private int _currentHealth;

    public int MaxHealth => _maxHealt;
    public int CurrentHealth => _currentHealth;

    [SerializeField] private Animator _animator;

    [SerializeField] private int _maxHealt = 100;
    [SerializeField] private FMODUnity.EventReference _sfxDamageEventRef;
    [SerializeField] private FMOD.Studio.EventInstance _sfxDamageInstance;
    [SerializeField] private DialogTypewritterComponent _globalMessageTypewritter;
    [SerializeField] private GameStateManager _gameState;

    private void Awake()
    {
        _currentHealth = _maxHealt;
        _sfxDamageInstance = FMODUnity.RuntimeManager.CreateInstance(_sfxDamageEventRef);
    }

    private void PlayDamageSFX()
    {
        if (_sfxDamageInstance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE playbackState) == FMOD.RESULT.OK)
        {
            if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
            {
                _sfxDamageInstance.start();
            }
        }
    }

    public void TakeDamage(int ammount)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= ammount;
        }

        Debug.Log("Player took " + ammount + " damage. Current health: " + _currentHealth);
        PlayDamageSFX();

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            StartCoroutine(Die());
        }
    }

    private void TeleportBack()
    {
        Debug.Log("registered event is running");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator Die()
    {
        _gameState.duringGameOverSplash = true;
        _animator.SetBool("IsDeath", true);

        yield return new WaitForSeconds(2f);

        if (_globalMessageTypewritter.CanStartSequence)
        {
            _globalMessageTypewritter.OnSequenceComplete += TeleportBack;
            _globalMessageTypewritter.EnqueueText("The stress was too much to handle...");
            _globalMessageTypewritter.EnqueueText("Your journey ends here...");
            _globalMessageTypewritter.EnqueueText("But don't give up!");
            _globalMessageTypewritter.EnqueueText("Take a deep breath, rest, and try again.");
            _globalMessageTypewritter.StartSequence();
        }
    }

    public void Heal(int ammount)
    {
        _currentHealth += ammount;
        _currentHealth = Mathf.Min(_currentHealth, _maxHealt);
        // _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealt);
        Debug.Log("Player healed " + ammount + " health. Current health: " + _currentHealth);
    }

    private void OnDestroy()
    {
        _sfxDamageInstance.release();
    }
}
