using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    public void Interact()
    {
        Debug.Log("Has interacted with " + gameObject.name + "!");
    }    
}
