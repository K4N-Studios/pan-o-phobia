using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameStateManager gameState;
    public KeyCode attackKeyCode = KeyCode.R;

    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackRate = 3f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private LayerMask _enemyLayer;
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

    // Player can only attack if he has the spray on hand
    private bool CanAttack()
    {
        return gameState.GetCollectable("CleaningSpray") != null;
    }

    public void HandleAttack()
    {
        Debug.Log("can attack -> " + CanAttack());

        if (!CanAttack())
        {
            return;
        }

        Collider2D enemy = Physics2D.OverlapCircle(transform.position, _attackRange, _enemyLayer);

        Debug.Log("enemy is found? -> " + enemy != null);

        if (enemy != null && enemy.TryGetComponent(out IDamageable target) && Input.GetKeyDown(attackKeyCode))
        {
            Debug.Log("collision!");
            var enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth.CurrentHealth <= 0)
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
