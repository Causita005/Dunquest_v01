using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Método para manejar las colisiones del proyectil
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el proyectil impacta contra el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Evita que el proyectil aplique fuerza sobre el jugador
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = Vector2.zero; // Elimina cualquier efecto de empuje en el jugador
            }

            // Destruye el proyectil al impactar contra el jugador
            Destroy(gameObject);
        }

        // Si el proyectil impacta contra una pared (Wall)
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Destruye el proyectil
            Destroy(gameObject);
        }
    }
}
