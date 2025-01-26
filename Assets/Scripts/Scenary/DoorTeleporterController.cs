using System.Collections;
using UnityEngine;

public class DoorTeleporterController : MonoBehaviour
{
    public GameObject firstDoor;
    public GameObject secondDoor;
    public GameObject player;

    private FadeController _fadeController;

    private void Awake()
    {
        _fadeController = FindObjectOfType<FadeController>();
    }

    private IEnumerator OpenSequence()
    {
        _fadeController.TriggerFade();
        yield return new WaitForSeconds(0.5f);
        player.transform.position = secondDoor.transform.position;
    }

    public void HandleOnOpen()
    {
        StartCoroutine(OpenSequence());
    }

    private IEnumerator OpenSequenceEnd()
    {
        _fadeController.TriggerFade();
        yield return new WaitForSeconds(0.5f);
        player.transform.position = firstDoor.transform.position;
    }

    // for second door
    public void HandleOnOpenEnd()
    {
        StartCoroutine(OpenSequenceEnd());
    }
}
