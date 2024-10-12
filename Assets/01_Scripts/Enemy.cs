using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    private Transform target;
    public EnemyType type;
    public float moveDistance = 1f; // Distancia que se moverá el enemigo en cada paso
    public float normalMoveCooldown = 2f; // Tiempo entre movimientos cuando no detecta al jugador
    public float chaseMoveCooldown = 0.5f; // Tiempo entre movimientos cuando detecta al jugador
    private float timer;
    public float detectionRange = 3f; // Rango de detección del jugador
    private Vector2 startPosition;
    private Vector2 endPosition;
    private bool movingUp = true; // Define la dirección del movimiento
    private bool playerInRange = false; // Verifica si el jugador está en rango

    public int health = 3; // Vida del enemigo, empezando con 3 golpes

    void Start()
    {
        // Encuentra al objeto del jugador usando el tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        timer = normalMoveCooldown;

        // Define la posición inicial y final del movimiento vertical
        startPosition = new Vector2(transform.position.x, -4); // Cambia -4 por la coordenada deseada
        endPosition = new Vector2(transform.position.x, 3); // Cambia 3 por la coordenada deseada
    }

    void Update()
    {
        // Solo procede si el tipo de enemigo es Queen
        if (type == EnemyType.Queen)
        {
            // Cuenta regresiva para los movimientos
            timer -= Time.deltaTime;

            // Verifica si el jugador está dentro del rango de detección
            playerInRange = target != null && Vector2.Distance(transform.position, target.position) <= detectionRange;

            // Si el temporizador llega a cero, intenta moverse
            if (timer <= 0)
            {
                if (playerInRange)
                {
                    // Si el jugador está dentro del rango de detección, se mueve hacia el jugador
                    MoveTowardsPlayer();
                    // Cambia el temporizador a un movimiento más rápido
                    timer = chaseMoveCooldown;
                }
                else
                {
                    // Si el jugador no está en rango, continúa con el patrullaje vertical
                    PatrolVertical();
                    // Cambia el temporizador al movimiento normal
                    timer = normalMoveCooldown;
                }
            }
        }
    }

    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        // Reducir la vida del enemigo
        health -= damage;
        Debug.Log("Enemy received damage. Remaining health: " + health);

        // Si la vida llega a 0, destruir el enemigo
        if (health <= 0)
        {
            Destroy(gameObject); // Destruye el enemigo
            Debug.Log("Enemy destroyed.");
        }
    }

    void PatrolVertical()
    {
        // Genera un número aleatorio entre 0 y 1
        float randomValue = Random.Range(0f, 1f);

        // 50% de probabilidad de moverse
        if (randomValue <= 0.5f)
        {
            // Mueve al enemigo hacia arriba o hacia abajo dependiendo de la dirección actual
            Vector2 targetPosition = movingUp ? endPosition : startPosition;

            // Mueve el enemigo hacia la posición objetivo
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveDistance);

            // Si ha alcanzado la posición objetivo, cambia la dirección
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                movingUp = !movingUp;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // Calcula la dirección hacia el jugador
        Vector2 direction = (target.position - transform.position).normalized;
        // Mueve el enemigo una distancia fija de "salto" hacia la dirección del jugador
        transform.position += (Vector3)(direction * moveDistance);
    }
}

// Enum to define the different types of enemies
public enum EnemyType
{
    Queen
}
