using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNinja : MonoBehaviour
{
    public float moveSpeed = 20f; // Velocidad al moverse entre puntos
    public float projectileSpeed = 5f; // Velocidad de los proyectiles
    public Transform firePointLeft; // Punto de disparo desde el lado izquierdo
    public Transform firePointRight; // Punto de disparo desde el lado derecho
    private Transform activeFirePoint; // Punto de disparo activo (izquierda o derecha)
    public GameObject projectilePrefab; // Prefab del proyectil
    public EnemySpawner spawner; // Referencia al spawner de enemigos
    private Transform target; // El objetivo (jugador)
    private List<Obstacule> obstacules = new List<Obstacule>(); // Lista de obstáculos
    private int currentObstaculeIndex = 0; // Índice del obstáculo actual en el que está el enemigo
    private bool isShooting = false; // Controla si el enemigo está disparando
    private bool isMoving = false; // Controla si el enemigo está en movimiento

    void Start()
    {
        // Encuentra al objeto del jugador usando el tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }

        // Encuentra todos los obstáculos por tag y los guarda en la lista
        obstacules.Add(GameObject.FindGameObjectWithTag("Obstacule1").GetComponent<Obstacule>());
        obstacules.Add(GameObject.FindGameObjectWithTag("Obstacule2").GetComponent<Obstacule>());
        obstacules.Add(GameObject.FindGameObjectWithTag("Obstacule3").GetComponent<Obstacule>());
        obstacules.Add(GameObject.FindGameObjectWithTag("Obstacule4").GetComponent<Obstacule>());
        obstacules.Add(GameObject.FindGameObjectWithTag("Obstacule5").GetComponent<Obstacule>()); // Agregamos Obstacule5

        // Inicia moviéndose hacia el primer obstáculo
        StartCoroutine(MoveToNextObstacule());

        // Comienza la cuenta regresiva de 5 segundos para la muerte automática del enemigo
        StartCoroutine(AutoDestroyEnemy());
    }

    void Update()
    {
        // Determina desde qué firePoint disparar basándose en el obstáculo actual
        if (currentObstaculeIndex == 0 || currentObstaculeIndex == 1) // Obstacule1 y Obstacule2 disparan hacia la derecha
        {
            activeFirePoint = firePointRight; // Disparar a la derecha
        }
        else if (currentObstaculeIndex >= 2) // Obstacule3, Obstacule4 y Obstacule5 disparan hacia la izquierda
        {
            activeFirePoint = firePointLeft; // Disparar a la izquierda
        }
    }

    // Método para mover el enemigo al siguiente obstáculo
    IEnumerator MoveToNextObstacule()
    {
        isMoving = true;
        Vector2 targetPosition = obstacules[currentObstaculeIndex].GetObstaculePoint(); // Obtiene el punto del obstáculo actual

        // Mueve al enemigo hacia el siguiente obstáculo
        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;

        // Cuando llega al obstáculo, dispara 3 veces y luego espera antes de moverse al siguiente
        StartCoroutine(ShootAndMoveDelay());
    }

    // Método que dispara 3 veces y luego espera antes de moverse
    IEnumerator ShootAndMoveDelay()
    {
        isShooting = true;

        // Dispara 3 veces
        for (int i = 0; i < 3; i++)
        {
            ShootProjectile();
            yield return new WaitForSeconds(0.1f); // Espera entre cada disparo
        }

        isShooting = false;

        // Espera un tiempo antes de moverse al siguiente obstáculo
        yield return new WaitForSeconds(0.2f);

        // Incrementa el índice para ir al siguiente obstáculo
        currentObstaculeIndex = (currentObstaculeIndex + 1) % obstacules.Count;

        // Mueve al enemigo al siguiente obstáculo
        StartCoroutine(MoveToNextObstacule());
    }

    // Método para disparar proyectiles en la dirección correcta desde el firePoint activo
    // Método para disparar proyectiles en la dirección correcta desde el firePoint activo
    void ShootProjectile()
    {
        if (projectilePrefab != null && activeFirePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, activeFirePoint.position, activeFirePoint.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            // Ignorar colisiones entre el proyectil y el enemigo
            Collider2D enemyCollider = GetComponent<Collider2D>();
            Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(projectileCollider, enemyCollider);

            // Si el enemigo está en Obstacule1 o Obstacule2 (disparar a la derecha)
            if (activeFirePoint == firePointRight)
            {
                rb.velocity = activeFirePoint.right * projectileSpeed; // Dispara hacia la derecha
            }
            // Si el enemigo está en Obstacule3, Obstacule4 o Obstacule5 (disparar a la izquierda)
            else if (activeFirePoint == firePointLeft)
            {
                rb.velocity = -activeFirePoint.right * projectileSpeed; // Dispara hacia la izquierda
            }

            // Destruye el proyectil si no impacta con nada después de 2 segundos
            Destroy(projectile, 2f);
        }
    }


    // Método para destruir automáticamente al enemigo después de 5 segundos (para probar)
    IEnumerator AutoDestroyEnemy()
    {
        yield return new WaitForSeconds(5f); // Espera 5 segundos
        Die(); // Llama al método Die() para destruir al enemigo
    }

    // Método para simular la muerte del enemigo
    public void Die()
    {
        // Notificar al spawner que el enemigo ha muerto
        if (spawner != null)
        {
            spawner.OnEnemyDeath();
        }

        // Destruir el enemigo
        Destroy(gameObject);
    }
}
