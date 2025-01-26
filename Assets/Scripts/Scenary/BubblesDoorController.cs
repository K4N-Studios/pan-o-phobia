using UnityEngine;

public class BubblesDoorController : MonoBehaviour
{
    public GameObject firstDoor;
    public GameObject bubbles;
    public GameObject player;

    public void HandleOnOpen()
    {
        bubbles.SetActive(false);
        gameObject.SetActive(false);
    }
}
