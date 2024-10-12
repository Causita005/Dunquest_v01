using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArcher : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator playerAnimation;
    [SerializeField] private float rayDistance = 1f;
    [SerializeField] private int rayCount = 16;
    private Vector2 moveInput;
    public float damage = 1f;
    public float bulletspeed = 5f;
    public Transform firePoint;
    public Bullet bulletPrefab;
    public float timeBtwShoot = 0.5f;
    float timer = 0;
    float life = 5;

    private BoxCollider2D playerCollider;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        HandleMovement();
        Shoot();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        // Animaciones de movimiento
        playerAnimation.SetFloat("Horizontal", moveX);
        playerAnimation.SetFloat("Vertical", moveY);
        playerAnimation.SetFloat("Speed", moveInput.sqrMagnitude);

        // Movimiento del jugador
        playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);

        // Ajusta la rotación del firePoint para que apunte en la dirección de movimiento
        if (moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    void Shoot()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && timer >= timeBtwShoot)
        {
            Bullet b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            b.damage = damage;
            b.speed = bulletspeed;
            timer = 0;
        }
    }
}


