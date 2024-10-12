using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacule : MonoBehaviour
{
    public Transform obstaculePoint; // El punto donde el enemigo se mover� si detecta este obst�culo

    void Start()
    {
        // Si no se ha asignado un obstaculePoint, muestra un mensaje de advertencia
        if (obstaculePoint == null)
        {
            Debug.LogWarning("No obstaculePoint assigned for " + gameObject.name + ". Please assign it in the Inspector.");
        }
    }

    // M�todo para obtener el punto hacia el que debe moverse el enemigo
    public Vector2 GetObstaculePoint()
    {
        // Si no se asign� manualmente un obstaculePoint, usa la posici�n del obst�culo
        if (obstaculePoint != null)
        {
            return obstaculePoint.position;
        }
        else
        {
            return transform.position; // Si no hay obstaculePoint, usa la posici�n del obst�culo
        }
    }
}
