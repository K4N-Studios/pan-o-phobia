using UnityEngine;

public class SepiaEffectManager : MonoBehaviour
{
    public GameObject sepiaElement;

    public void Enable()
    {
        sepiaElement.SetActive(true);
    }

    public void Disable()
    {
        sepiaElement.SetActive(false);
    }

    public void ToggleVisibility()
    {
        sepiaElement.SetActive(!sepiaElement.activeSelf);
    }
}
