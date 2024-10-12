using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float normalJumpSpeed = 40f; // Velocidad normal del salto
    public float increasedJumpSpeed = 60f; // Velocidad aumentada cuando detecta al jugador
    public float detectCooldown = 0.5f; // Cada cuánto tiempo detecta la posición del jugador (más rápido)
    public float jumpDelay = 0.3f; // Tiempo que espera antes de saltar (más rápido)
    private Transform target;
    private Vector2 targetPosition;
    private bool isJumping = false;
    private float detectTimer;
    private float jumpTimer;
    public BossType type;
    public Transform firePoint; // Punto de disparo desde el centro del boss
    public GameObject projectilePrefab; // Prefab del proyectil
    public float projectileSpeed = 8f; // Velocidad reducida de los proyectiles
    private int jumpCount = 0; // Contador de saltos
    public int jumpsBeforeShooting = 3; // Número de saltos antes de disparar

    public int health = 20; // Vida del boss, empezando con 3 golpes

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
        // Solo procede si el tipo de boss es Pawn y hay un objetivo (el jugador)
        if (type == BossType.Pawn && target != null)
        {
            // Cuenta regresiva para la detección de la posición del jugador
            detectTimer -= Time.deltaTime;

            // Si el temporizador de detección llega a cero, obtiene la posición del jugador
            if (detectTimer <= 0)
            {
                targetPosition = target.position;
                isJumping = false; // Indica que todavía no ha saltado
                jumpTimer = jumpDelay; // Resetea el tiempo de espera antes de saltar
                detectTimer = detectCooldown; // Resetea el temporizador de detección
            }

            // Si la posición del jugador ha sido detectada, empieza la cuenta regresiva para el salto
            if (!isJumping)
            {
                jumpTimer -= Time.deltaTime;

                // Si el temporizador de salto llega a cero, salta a la posición del jugador
                if (jumpTimer <= 0)
                {
                    StartCoroutine(JumpToTarget());
                    isJumping = true;
                }
            }
        }
    }

    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        // Reducir la vida del boss
        health -= damage;
        Debug.Log("Boss received damage. Remaining health: " + health);

        // Si la vida llega a 0, destruir el boss
        if (health <= 0)
        {
            Destroy(gameObject); // Destruye el boss
            Debug.Log("Boss destroyed.");
        }
    }

    IEnumerator JumpToTarget()
    {
        // Usa la velocidad aumentada cuando se mueve hacia el jugador
        float jumpSpeed = increasedJumpSpeed;

        // Calcula la dirección hacia la posición objetivo
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        // Mueve al boss hacia la posición objetivo rápidamente
        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, jumpSpeed * Time.deltaTime);
            yield return null;
        }

        // Incrementa el contador de saltos
        jumpCount++;

        // Si el contador de saltos alcanza el valor definido, dispara proyectiles
        if (jumpCount >= jumpsBeforeShooting)
        {
            ShootProjectiles();
            jumpCount = 0; // Resetea el contador de saltos
        }
    }

    void ShootProjectiles()
    {
        int projectileCount = 10;
        float angleStep = 360f / projectileCount; // Distribuye los ángulos de manera uniforme

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * angleStep;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            // Instancia un proyectil y le da la dirección calculada
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction * projectileSpeed;

            // Ignorar colisiones entre el proyectil y el boss
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            // Destruye el proyectil después de 5 segundos
            Destroy(projectile, 5f);
        }
    }
}

// Enum to define the different types of bosses
public enum BossType
{
    Pawn
}
