using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Asigna el transform del jugador en el inspector
    public float smoothSpeed = 0.125f; // Velocidad de suavizado
    public Vector3 offset; // Offset para posicionar la c�mara

    void Start()
    {
        // Puedes inicializar cualquier cosa aqu� si es necesario
    }

    void LateUpdate()
    {
        if (player != null) // Aseg�rate de que el jugador est� asignado
        {
            // Define la posici�n deseada de la c�mara
            Vector3 desiredPosition = player.position + offset;

            // Suaviza la transici�n de la c�mara
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Asigna la nueva posici�n a la c�mara
            transform.position = smoothedPosition;
        }
    }
}