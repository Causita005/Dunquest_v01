using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNinja : MonoBehaviour
{
    public float runAwaySpeed = 5f; // Velocidad al huir del jugador
    public float moveDistance = 3f; // Distancia m�nima que se mover� el enemigo al alejarse
    public float restTime = 3f; // Tiempo de descanso antes de repetir el ciclo (ajustado a 2 segundos)
    public GameObject projectilePrefab; // Prefab del proyectil
    public Transform firePoint; // Punto de disparo desde el centro del enemigo
    public float projectileSpeed = 15f; // Velocidad de los proyectiles
    private Transform target;
    private int escapeAttempts = 0; // Intentos de escapar y disparar
    private bool isTired = false; // Indica si el enemigo est� cansado
    private float timer;
    private bool isMoving = false; // Controla si el enemigo est� en proceso de moverse
    public Vector2 mapMinBounds; // Coordenadas m�nimas del mapa
    public Vector2 mapMaxBounds; // Coordenadas m�ximas del mapa
    private float detectCooldown = 2f; // Frecuencia de disparo (ajustado a 1 segundo)

    public int health = 3; // Vida del enemigo Ninja, empezando con 3 golpes

    void Start()
    {
        // Encuentra al objeto del jugador usando el tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        timer = 0f;
    }

    void Update()
    {
        if (target == null) return;

        // Si el enemigo est� cansado, espera 2 segundos antes de reiniciar el ciclo
        if (isTired)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                isTired = false;
                escapeAttempts = 0; // Reinicia el ciclo despu�s del descanso
                isMoving = false;
            }
            return;
        }

        // Si no est� movi�ndose ni cansado, realiza la siguiente acci�n
        if (!isMoving)
        {
            if (escapeAttempts < 3)
            {
                // Dispara un proyectil hacia el jugador
                ShootProjectile();

                // Inicia el movimiento hacia una nueva ubicaci�n aleatoria lejos del jugador
                StartCoroutine(MoveToRandomLocation());
            }
            else
            {
                // Si ha realizado 3 intentos, se cansa y se queda quieto
                isTired = true;
                timer = restTime; // Espera 2 segundos antes de reiniciar el ciclo
            }
        }
    }

    // M�todo para recibir da�o
    public void TakeDamage(int damage)
    {
        // Reducir la vida del enemigo
        health -= damage;
        Debug.Log("Ninja received damage. Remaining health: " + health);

        // Si la vida llega a 0, destruir el enemigo
        if (health <= 0)
        {
            Destroy(gameObject); // Destruye el enemigo
            Debug.Log("Ninja destroyed.");
        }
    }

    IEnumerator MoveToRandomLocation()
    {
        isMoving = true;

        // Calcula una nueva posici�n aleatoria dentro de los l�mites del mapa y lejos del jugador
        Vector2 randomDirection = (Random.insideUnitCircle.normalized); // Direcci�n aleatoria
        Vector2 newPosition = (Vector2)transform.position + randomDirection * moveDistance;

        // Aseg�rate de que la nueva posici�n est� dentro de los l�mites del mapa
        newPosition.x = Mathf.Clamp(newPosition.x, mapMinBounds.x, mapMaxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, mapMinBounds.y, mapMaxBounds.y);

        // Mueve al enemigo hacia la nueva posici�n
        while (Vector2.Distance(transform.position, newPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, newPosition, runAwaySpeed * Time.deltaTime);
            yield return null;
        }

        // Incrementa el n�mero de intentos de escape
        escapeAttempts++;
        isMoving = false; // Permite que el ciclo contin�e
        timer = detectCooldown; // Resetea el temporizador de disparo a 1 segundo
    }

    void ShootProjectile()
    {
        // Instancia un proyectil y le da la direcci�n hacia el jugador
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            Vector2 direction = (target.position - firePoint.position).normalized;
            rb.velocity = direction * projectileSpeed;

            // Destruye el proyectil despu�s de 5 segundos para evitar que se quede en la escena
            Destroy(projectile, 5f);
        }
    }
}
