using UnityEngine;

public class BubbleEffectUI : MonoBehaviour
{
    public float speed = 2.0f; // Velocidad del movimiento
    public float amplitude = 1.0f; // Amplitud del movimiento (cuánto se mueve arriba y abajo)
    private RectTransform rectTransform;
    private Vector3 initialPosition;

    void Start()
    {
        // Obtén el RectTransform del objeto
        rectTransform = GetComponent<RectTransform>();

        // Guarda la posición inicial del objeto
        initialPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        // Calcula el nuevo valor de Y utilizando Mathf.PingPong
        float newY = initialPosition.y + Mathf.PingPong(Time.time * speed, amplitude);

        // Aplica la nueva posición al objeto
        rectTransform.anchoredPosition = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}
