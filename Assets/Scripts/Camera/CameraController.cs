using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("Which object the camera should follow.")]
    public Transform target;

    [Header("Camera Settings")]
    [Tooltip("Optional offset to apply to the camera.")]
    public Vector3 offset;

    private void LateUpdate()
    {
        // Verifica si el objetivo está asignado
        if (target == null)
        {
            Debug.LogWarning("No target assigned to the camera controller.");
            return;
        }

        // Calcula la nueva posición de la cámara
        Vector3 targetPosition = target.position + offset;

        // Mueve la cámara directamente a la posición objetivo
        transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
    }
}
