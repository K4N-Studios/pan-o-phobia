using TMPro;
using UnityEngine;

public class ControlHint : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _hintText;

    private void Start()
    {
        _panel.SetActive(false);
    }

    public void SetHint(string text, bool isActive)
    {
        Debug.Log("Setting hint: " + text);
        Debug.Log("Setting hint active: " + isActive);
        _hintText.text = text;
        _panel.SetActive(isActive);
    }
}
