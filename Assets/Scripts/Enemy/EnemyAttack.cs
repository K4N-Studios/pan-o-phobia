using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackRate = 3f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private FMODUnity.EventReference _sfxAttackEventRef;
    [SerializeField] private FMOD.Studio.EventInstance _sfxAttackInstance;

    private void Awake()
    {
        _sfxAttackInstance = FMODUnity.RuntimeManager.CreateInstance(_sfxAttackEventRef);
    }

    private void PlayAttackSFX()
    {
        if (_sfxAttackInstance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE playbackState) == FMOD.RESULT.OK)
        {
            if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
            {
                _sfxAttackInstance.start();
            }
        }
    }

    public void HandleAttack()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, _attackRange, _playerLayer);

        if (player != null && player.TryGetComponent<IDamageable>(out IDamageable target))
        {
            if (player.GetComponent<PlayerHealth>().CurrentHealth <= 0)
            {
                return;
            }

            if (Time.time >= _attackRate)
            {
                _attackRate = Time.time + 1f;
                target.TakeDamage(_damage);
                PlayAttackSFX();
            }
        }
    }

    private void OnDestroy()
    {
        _sfxAttackInstance.release();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
