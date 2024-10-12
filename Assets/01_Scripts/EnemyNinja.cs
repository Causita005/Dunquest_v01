using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNinja : MonoBehaviour
{
    public float moveSpeed = 100f; // Velocidad al moverse entre puntos, más rápida
    public float shootInterval = 0.1f; // Intervalo entre disparos, más rápido
    public float moveDelay = 0.2f; // Tiempo que espera antes de moverse al siguiente obstáculo después de disparar, más rápido
    public GameObject projectilePrefab; // Prefab del proyectil
    public Transform firePoint; // Punto de disparo desde el centro del enemigo
    public float projectileSpeed = 100f; // Velocidad de los proyectiles, aún más rápida
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
    }

    void Update()
    {
        // Aseguramos que el firePoint siempre apunte hacia el lado donde está el jugador
        if (target != null && firePoint != null)
        {
            // Si el jugador está a la derecha, el firePoint apunta a la derecha
            if (target.position.x > transform.position.x)
            {
                firePoint.rotation = Quaternion.Euler(0f, 0f, 0f); // Apunta a la derecha
            }
            // Si el jugador está a la izquierda, el firePoint apunta a la izquierda
            else if (target.position.x < transform.position.x)
            {
                firePoint.rotation = Quaternion.Euler(0f, 0f, 180f); // Apunta a la izquierda
            }
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
            yield return new WaitForSeconds(shootInterval); // Espera entre cada disparo
        }

        isShooting = false;

        // Espera un tiempo antes de moverse al siguiente obstáculo
        yield return new WaitForSeconds(moveDelay);

        // Incrementa el índice para ir al siguiente obstáculo
        currentObstaculeIndex = (currentObstaculeIndex + 1) % obstacules.Count;

        // Mueve al enemigo al siguiente obstáculo
        StartCoroutine(MoveToNextObstacule());
    }

    // Método para disparar proyectiles hacia la dirección del firePoint (dependiendo de la rotación)
    void ShootProjectile()
    {
        // Instancia un proyectil y lo dispara en la dirección del firePoint
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            // Aplica la velocidad en la dirección del eje X del firePoint (dependiendo de su rotación)
            rb.velocity = firePoint.right * projectileSpeed;

            // Destruye el proyectil si no impacta con nada después de 2 segundos
            Destroy(projectile, 2f);
        }
    }
}
