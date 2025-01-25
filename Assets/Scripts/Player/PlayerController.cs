using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerInteraction _playerInteraction;

    private void Update()
    {
        _playerMovement.HandleMovementInput();
        _playerInteraction.HandleInteractionInput();
        _playerMovement.HandleFlashLightMovement();
    }
}
