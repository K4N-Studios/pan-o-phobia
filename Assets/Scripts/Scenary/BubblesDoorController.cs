using UnityEngine;

public class BubblesDoorController : MonoBehaviour
{
    public GameObject firstDoor;
    public GameObject secondDoor;
    public GameObject player;

    public void HandleOnOpen()
    {
        player.transform.position = secondDoor.transform.position;
    }

    // for second door
    public void HandleOnOpenEnd()
    {
        player.transform.position = firstDoor.transform.position;
    }
}
