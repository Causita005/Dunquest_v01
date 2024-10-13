using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo que se va a instanciar
    public Transform[] spawnPoints; // Puntos de spawn en el mapa para los enemigos
    private int enemyCount = 1; // Contador para el número de enemigos generados
    private float initialMoveSpeed = 20f; // Velocidad inicial del enemigo
    private float initialProjectileSpeed = 5f; // Velocidad inicial de los proyectiles

    void Start()
    {
        // Genera el primer enemigo al inicio
        SpawnNewEnemy();
    }

    // Método para generar un nuevo enemigo en un punto aleatorio
    public void SpawnNewEnemy()
    {
        if (enemyCount > 5)
        {
            Debug.Log("Límite máximo de velocidad alcanzado.");
            return;
        }

        // Seleccionar un punto de spawn aleatorio
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instanciar un nuevo enemigo en el punto de spawn
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // Obtener el script del enemigo y ajustar las velocidades
        EnemyNinja enemyScript = newEnemy.GetComponent<EnemyNinja>();
        if (enemyScript != null)
        {
            // Ajustar las velocidades del nuevo enemigo
            enemyScript.moveSpeed = initialMoveSpeed + (enemyCount - 1) * 20f; // Incremento de velocidad de 20
            enemyScript.projectileSpeed = initialProjectileSpeed + (enemyCount - 1) * 5f; // Incremento de velocidad del proyectil de 5

            // Asignar el spawner para que el enemigo pueda notificar al morir
            enemyScript.spawner = this;
        }

        // Incrementar el contador de enemigos
        enemyCount++;
    }

    // Método para simular la muerte del enemigo y generar otro
    public void OnEnemyDeath()
    {
        SpawnNewEnemy();
    }
}
