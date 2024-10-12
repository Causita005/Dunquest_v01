using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNinja : MonoBehaviour
{
    public float moveSpeed = 100f; // Velocidad al moverse entre puntos, m�s r�pida
    public float shootInterval = 0.1f; // Intervalo entre disparos, m�s r�pido
    public float moveDelay = 0.2f; // Tiempo que espera antes de moverse al siguiente obst�culo despu�s de disparar, m�s r�pido
    public GameObject projectilePrefab; // Prefab del proyectil
    public Transform firePoint; // Punto de disparo desde el centro del enemigo
    public float projectileSpeed = 100f; // Velocidad de los proyectiles, a�n m�s r�pida
    private Transform target; // El objetivo (jugador)
    private List<Obstacule> obstacules = new List<Obstacule>(); // Lista de obst�culos
    private int currentObstaculeIndex = 0; // �ndice del obst�culo actual en el que est� el enemigo
    private bool isShooting = false; // Controla si el enemigo est� disparando
    private bool isMoving = false; // Controla si el enemigo est� en movimiento

    void Start()
    {
        // Encuentra al objeto del jugador usando el tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }

        // Encuentra todos los obst�culos por tag y los guarda en la lista
        obstacules.Add(GameObject.FindGameObjectWithTag("Obstacule1").GetComponent<Obstacule>());
        obstacules.Add(GameObject.FindGameObjectWithTag("Obstacule2").GetComponent<Obstacule>());
        obstacules.Add(GameObject.FindGameObjectWithTag("Obstacule3").GetComponent<Obstacule>());
        obstacules.Add(GameObject.FindGameObjectWithTag("Obstacule4").GetComponent<Obstacule>());
        obstacules.Add(GameObject.FindGameObjectWithTag("Obstacule5").GetComponent<Obstacule>()); // Agregamos Obstacule5

        // Inicia movi�ndose hacia el primer obst�culo
        StartCoroutine(MoveToNextObstacule());
    }

    void Update()
    {
        // Aseguramos que el firePoint siempre apunte hacia el lado donde est� el jugador
        if (target != null && firePoint != null)
        {
            // Si el jugador est� a la derecha, el firePoint apunta a la derecha
            if (target.position.x > transform.position.x)
            {
                firePoint.rotation = Quaternion.Euler(0f, 0f, 0f); // Apunta a la derecha
            }
            // Si el jugador est� a la izquierda, el firePoint apunta a la izquierda
            else if (target.position.x < transform.position.x)
            {
                firePoint.rotation = Quaternion.Euler(0f, 0f, 180f); // Apunta a la izquierda
            }
        }
    }

    // M�todo para mover el enemigo al siguiente obst�culo
    IEnumerator MoveToNextObstacule()
    {
        isMoving = true;
        Vector2 targetPosition = obstacules[currentObstaculeIndex].GetObstaculePoint(); // Obtiene el punto del obst�culo actual

        // Mueve al enemigo hacia el siguiente obst�culo
        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;

        // Cuando llega al obst�culo, dispara 3 veces y luego espera antes de moverse al siguiente
        StartCoroutine(ShootAndMoveDelay());
    }

    // M�todo que dispara 3 veces y luego espera antes de moverse
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

        // Espera un tiempo antes de moverse al siguiente obst�culo
        yield return new WaitForSeconds(moveDelay);

        // Incrementa el �ndice para ir al siguiente obst�culo
        currentObstaculeIndex = (currentObstaculeIndex + 1) % obstacules.Count;

        // Mueve al enemigo al siguiente obst�culo
        StartCoroutine(MoveToNextObstacule());
    }

    // M�todo para disparar proyectiles hacia la direcci�n del firePoint (dependiendo de la rotaci�n)
    void ShootProjectile()
    {
        // Instancia un proyectil y lo dispara en la direcci�n del firePoint
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            // Aplica la velocidad en la direcci�n del eje X del firePoint (dependiendo de su rotaci�n)
            rb.velocity = firePoint.right * projectileSpeed;

            // Destruye el proyectil si no impacta con nada despu�s de 2 segundos
            Destroy(projectile, 2f);
        }
    }
}
