using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _elapsedTime = 0f;
    [SerializeField] private float _fadeDuration = 1f;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("Se presionó una tecla. Y empieza el juego");
            StartGameplay();
            // Aquí puedes agregar la acción que deseas ejecutar
        }
    }

    private void StartGameplay () {
        Debug.Log("start");

        StartCoroutine(FadeToZero());
    }

    private IEnumerator FadeToZero()
    {
        Debug.Log("Fade");

        float startAlpha = _canvasGroup.alpha;

        while (_elapsedTime < _fadeDuration)
        {
            _elapsedTime += Time.deltaTime;

            _canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, _elapsedTime / _fadeDuration);
            yield return null;
        }

        // Asegúrate de que el alpha sea exactamente 0 al final
        _canvasGroup.alpha = 0f;
    }
}
