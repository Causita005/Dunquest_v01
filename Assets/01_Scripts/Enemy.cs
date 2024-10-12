using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    private Transform target;
    public EnemyType type;
    public float moveDistance = 1f; // Distancia que se moverá el enemigo si la probabilidad es favorable
    private float moveCooldown = 1f; // Tiempo entre intentos de movimiento
    private float timer;

    void Start()
    {
        // Find the player object by its tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        timer = moveCooldown;
    }

    void Update()
    {
        // Only proceed if the enemy type is Queen and the target exists
        if (type == EnemyType.Queen && target != null)
        {
            // Countdown timer
            timer -= Time.deltaTime;

            // If the timer reaches zero, attempt to move
            if (timer <= 0)
            {
                TryToMove();
                // Reset the timer
                timer = moveCooldown;
            }
        }
    }

    void TryToMove()
    {
        // Generate a random number between 0 and 1
        float randomValue = Random.Range(0f, 1f);

        // 30% chance to move towards the player
        if (randomValue <= 0.5f)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate direction towards the player
        Vector2 direction = (target.position - transform.position).normalized;
        // Move the enemy a set distance in the direction of the player
        transform.position += (Vector3)(direction * moveDistance);
        // Rotate to face the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }
}

// Enum to define the different types of enemies
public enum EnemyType
{
    Queen
}
