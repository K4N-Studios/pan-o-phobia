using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private PlayerAttack _playerAttack;

    private void Update()
    {
        _playerMovement.HandleMovementInput();
        _playerInteraction.HandleInteractionInput();
        _playerMovement.HandleFlashLightMovement();
        _playerAttack.HandleAttack();
    }
}
