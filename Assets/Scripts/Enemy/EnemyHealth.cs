using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private int _currentHealth;

    public int CurrentHealth => _currentHealth;

    [SerializeField] private int _maxHealth = 30;
    [SerializeField] private FMODUnity.EventReference _sfxDamageEventRef;
    [SerializeField] private FMOD.Studio.EventInstance _sfxDamageInstance;

    private void Awake()
    {
        _sfxDamageInstance = FMODUnity.RuntimeManager.CreateInstance(_sfxDamageEventRef);
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
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

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        Debug.Log("Enemy took " + amount + " damage. Current health: " + _currentHealth);
        PlayDamageSFX();

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy has died");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _sfxDamageInstance.release();
    }
}
