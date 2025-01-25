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

    private void ExcecuteInteraction()
    {
        Collider2D interactable = Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Collectable"));
        if (interactable != null && interactable.TryGetComponent<Interactable>(out Interactable target))
        {
            target.Interact();
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
