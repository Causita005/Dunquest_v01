using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float jumpSpeed = 15f; // Velocidad a la que el boss se mover� al saltar
    public float detectCooldown = 2f; // Cada cu�nto tiempo detecta la posici�n del jugador
    public float jumpDelay = 0.5f; // Tiempo que espera antes de saltar
    private Transform target;
    private Vector2 targetPosition;
    private bool isJumping = false;
    private float detectTimer;
    private float jumpTimer;
    public BossType type;

    void Start()
    {
        // Encuentra al objeto del jugador usando el tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        detectTimer = detectCooldown;
    }

    void Update()
    {
        // Solo procede si el tipo de enemigo es Pawn y hay un objetivo (el jugador)
        if (type == BossType.Pawn && target != null)
        {
            // Cuenta regresiva para la detecci�n de la posici�n del jugador
            detectTimer -= Time.deltaTime;

            // Si el temporizador de detecci�n llega a cero, obtiene la posici�n del jugador
            if (detectTimer <= 0)
            {
                targetPosition = target.position;
                isJumping = false; // Indica que todav�a no ha saltado
                jumpTimer = jumpDelay; // Resetea el tiempo de espera antes de saltar
                detectTimer = detectCooldown; // Resetea el temporizador de detecci�n
            }

            // Si la posici�n del jugador ha sido detectada, empieza la cuenta regresiva para el salto
            if (!isJumping)
            {
                jumpTimer -= Time.deltaTime;

                // Si el temporizador de salto llega a cero, salta a la posici�n del jugador
                if (jumpTimer <= 0)
                {
                    StartCoroutine(JumpToTarget());
                    isJumping = true;
                }
            }
        }
    }

    IEnumerator JumpToTarget()
    {
        // Calcula la direcci�n hacia la posici�n objetivo
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        // Mueve al boss hacia la posici�n objetivo r�pidamente
        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, jumpSpeed * Time.deltaTime);
            yield return null;
        }
    }
}

// Enum to define the different types of enemies
public enum BossType
{
    Pawn
}

