using System.Collections;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : WithSongManager, IDamageable
{
    private int _currentHealth;

    public int MaxHealth => _maxHealt;
    public int CurrentHealth => _currentHealth;

    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerAudioManager _audioManager;

    [SerializeField] private int _maxHealt = 100;
    [SerializeField] private DialogTypewritterComponent _globalMessageTypewritter;
    [SerializeField] private GameStateManager _gameState;

    [Header("Songs")]
    [SerializeField] private SoundType _damageSound = SoundType.PlayerDamage;
    [SerializeField] private SoundType _collapseSound = SoundType.PlayerCollapse;

    private void Start()
    {
        _currentHealth = _maxHealt;
    }

    public void TakeDamage(int ammount)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= ammount;
        }

        _soundManager.Play(_damageSound);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            StartCoroutine(Die());
        }
    }

    private void RestartGame()
    {
        _soundManager.ReleaseAndStopAll(fadeout: true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void CreateDyingScene()
    {
        if (_globalMessageTypewritter.CanStartSequence)
        {
            _globalMessageTypewritter.OnSequenceComplete += RestartGame;
            _globalMessageTypewritter.EnqueueText("The stress was too much to handle...");
            _globalMessageTypewritter.EnqueueText("Your journey ends here...");
            _globalMessageTypewritter.EnqueueText("But don't give up!");
            _globalMessageTypewritter.EnqueueText("Take a deep breath, rest, and try again.");
            _globalMessageTypewritter.StartSequence();
        }
    }

    private IEnumerator Die()
    {
        _gameState.duringGameOverSplash = true;
        _animator.SetBool("IsDeath", true);

        yield return new WaitForSeconds(2f);

        CreateDyingScene();

        yield return new WaitForSeconds(1f);

        _audioManager.FadeOutHeartbeat(4f);
        _soundManager.Play(_collapseSound);
    }

    public void Heal(int ammount)
    {
        _currentHealth += ammount;
        _currentHealth = Mathf.Min(_currentHealth, _maxHealt);
        // _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealt);
        Debug.Log("Player healed " + ammount + " health. Current health: " + _currentHealth);
    }
}
