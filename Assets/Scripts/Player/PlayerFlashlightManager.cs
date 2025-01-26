using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerFlashlightManager : MonoBehaviour
{
    public GameObject flashlightCollectable;
    public Light2D flashlightObject;
    public GameStateManager gameState;
    public KeyCode toggleKeyCode = KeyCode.F;

    [SerializeField] private bool _locked = true;

    private void SetFlashlightActive(bool value)
    {
        if (!flashlightObject) return;
        flashlightObject.gameObject.SetActive(value);
    }

    private void Start()
    {
        if (_locked)
        {
            SetFlashlightActive(false);
        }
    }

    private void TrynaUnlock()
    {
        var flashlight = gameState.GetCollectable(flashlightCollectable.name);
        var hasFlashlight = flashlight != null && flashlight.Amount >= 1;

        if (hasFlashlight)
        {
            _locked = false;
        }
    }

    private void HandleKeys()
    {
        if (Input.GetKeyDown(toggleKeyCode))
        {
            SetFlashlightActive(!flashlightObject.gameObject.activeSelf);
        }
    }

    private void Update()
    {
        if (_locked)
        {
            TrynaUnlock();
        }

        HandleKeys();
    }
}
