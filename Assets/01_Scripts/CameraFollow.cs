using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Asigna el transform del jugador en el inspector
    public float smoothSpeed = 0.125f; // Velocidad de suavizado
    public Vector3 offset; // Offset para posicionar la cámara

    void Start()
    {
        // Puedes inicializar cualquier cosa aquí si es necesario
    }

    void LateUpdate()
    {
        if (player != null) // Asegúrate de que el jugador esté asignado
        {
            // Define la posición deseada de la cámara
            Vector3 desiredPosition = player.position + offset;

            // Suaviza la transición de la cámara
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Asigna la nueva posición a la cámara
            transform.position = smoothedPosition;
        }
    }
}