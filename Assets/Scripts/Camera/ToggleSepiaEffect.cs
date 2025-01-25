using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ToggleSepiaEffect : MonoBehaviour
{
    [SerializeField]
    private ScriptableRendererFeature _sepiaRendererFeature;
    private bool _isSepiaActive = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            _isSepiaActive = !_isSepiaActive;
            _sepiaRendererFeature.SetActive(_isSepiaActive);
        }
    }
}
