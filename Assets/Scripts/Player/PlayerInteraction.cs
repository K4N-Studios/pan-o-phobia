using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
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
            Debug.Log("Starting timer of timed switch");
            timedSwitchBehavior.ToggleSwitch();
        }
    }

    private void InteractWithCollectable(GameObject collectable)
    {
        Debug.Log("collecting collectable: " + collectable);
        collectable.SetActive(false);
    }

    private void ExcecuteInteraction()
    {
        Collider2D interactable = Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Collectable") | LayerMask.GetMask("Interactable"));
        if (interactable != null && interactable.TryGetComponent(out Interactable target))
        {
            GameObject gameObject = target.Interact();

            var interactableMask = LayerMask.NameToLayer("Interactable");
            var collectableMask = LayerMask.NameToLayer("Collectable");

            if (gameObject.layer == interactableMask) InteractWithInteractable(gameObject);
            else if (gameObject.layer == collectableMask) InteractWithCollectable(gameObject);
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
