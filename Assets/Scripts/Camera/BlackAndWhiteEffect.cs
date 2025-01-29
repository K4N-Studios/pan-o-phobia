using UnityEngine;
using UnityEngine.Rendering;

public class BlackAndWhiteEffect : MonoBehaviour
{
    public Volume postProcessingVolume;
    private bool _isEffectActive = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleEffect();
        }
    }

    public void ToggleEffect()
    {
        _isEffectActive = !_isEffectActive;
        postProcessingVolume.weight = _isEffectActive ? 1 : 0;
    }
}
