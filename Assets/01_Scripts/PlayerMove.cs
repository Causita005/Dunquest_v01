using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 3f; // Para ajustar la velocidad desde el inspector
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator playerAnimation;
    [SerializeField] private float rayDistance = 1f; // Distancia del raycast
    [SerializeField] private int rayCount = 16; // Número de rayos para cubrir el círculo completo
    private Vector2 moveInput;

    private BoxCollider2D playerCollider;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        playerAnimation.SetFloat("Horizontal", moveX);
        playerAnimation.SetFloat("Vertical", moveY);
        playerAnimation.SetFloat("Speed", moveInput.sqrMagnitude);

        playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);
    }
}
