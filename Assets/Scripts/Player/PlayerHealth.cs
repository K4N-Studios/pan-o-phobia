using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private int _currentHealth;

    public int MaxHealth => _maxHealt;
    public int CurrentHealth => _currentHealth;

    [SerializeField] private int _maxHealt = 100;
    [SerializeField] private FMODUnity.EventReference _sfxDamageEventRef;
    [SerializeField] private FMOD.Studio.EventInstance _sfxDamageInstance;

    private void Awake()
    {
        _currentHealth = _maxHealt;
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
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died");
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
