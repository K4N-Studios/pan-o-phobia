using UnityEngine;

public class Interactable : MonoBehaviour
{
    private ControlHint _controlHint;
    private bool _shouldDisplayControlHint = false;

    private void Awake()
    {
        _controlHint = FindObjectOfType<ControlHint>();
    }

    private void DrawInteractionMark()
    {
        var playerMask = LayerMask.GetMask("Player");
        var playerCollision = Physics2D.OverlapCircle(transform.position, 1f, playerMask);

        if (playerCollision != null && !_shouldDisplayControlHint)
        {
            _shouldDisplayControlHint = true;
            _controlHint.SetHint("Interact with " + gameObject.name, _shouldDisplayControlHint);
        }
        else if (playerCollision == null && _shouldDisplayControlHint)
        {
            _shouldDisplayControlHint = false;
            _controlHint.SetHint("", _shouldDisplayControlHint);
        }
    }

    private void Update()
    {
        DrawInteractionMark();
    }

    public GameObject Interact()
    {
        _controlHint.SetHint("", false);
        return gameObject;
    }
}
