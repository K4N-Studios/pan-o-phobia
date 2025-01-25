using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    public GameObject Interact()
    {
        Debug.Log("Has interacted with " + gameObject.name + "!");
        return gameObject;
    }    
}
