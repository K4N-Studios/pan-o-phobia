using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameStateManager gameState;

    public void HandleInteractionInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExcecuteInteraction();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ExcecuteRecoverHealth();
        }
    }

    private void InteractWithInteractable(GameObject interactable)
    {
        if (interactable.TryGetComponent(out LightSwitchInteractableBehavior lightSwitchBehavior))
        {
            lightSwitchBehavior.ToggleSwitch();
        }

        if (interactable.TryGetComponent(out TimedSwitchInteractableBehavior timedSwitchBehavior))
        {
            timedSwitchBehavior.ToggleSwitch();
        }

        if (interactable.TryGetComponent(out DoorInteractableBehavior doorBehavior))
        {
            doorBehavior.OpenDoor();
        }
    }

    private void InteractWithCollectable(GameObject collectable)
    {
        gameState.RegisterCollectable(collectable.name);
        collectable.SetActive(false);
    }

    private void ExcecuteInteraction()
    {
        var desiredMasks = LayerMask.GetMask("Collectable") | LayerMask.GetMask("Interactable");
        var interactable = Physics2D.OverlapCircle(transform.position, 0.5f, desiredMasks);

        if (interactable != null && interactable.TryGetComponent(out Interactable target))
        {
            GameObject gameObject = target.Interact();
            
            if (gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                InteractWithInteractable(gameObject);
            }
            else if (gameObject.layer == LayerMask.NameToLayer("Collectable"))
            {
                InteractWithCollectable(gameObject);
            }
        }
    }

    private void ExcecuteRecoverHealth()
    {
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth.CurrentHealth < playerHealth.MaxHealth)
        {
            playerHealth.Heal(30);
        }
        
        return;
    }
}
