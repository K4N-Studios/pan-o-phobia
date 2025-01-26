using UnityEngine;

public class Interactable : MonoBehaviour
{
    private void DrawInteractionMark()
    {
        var playerMask = LayerMask.GetMask("Player");
        var playerCollision = Physics2D.OverlapCircle(transform.position, 1f, playerMask);

        if (playerCollision != null)
        {
            Debug.Log("Player is being able to interact with element: " + gameObject.name);
        }
    }

    private void Update()
    {
        DrawInteractionMark();
    }

    public GameObject Interact()
    {
        return gameObject;
    }
}
