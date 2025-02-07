using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private PlayerStress _playerStress;
    [SerializeField] private PlayerAudioManager _playerAudioManager;

    private void Update()
    {
        _playerMovement.HandleMovementInput();
        _playerInteraction.HandleInteractionInput();
        _playerMovement.HandleFlashLightMovement();
        _playerAttack.HandleAttack();
        _playerStress.UpdateStressTimer();
        _playerAudioManager.CheckSounds();
    }
}
