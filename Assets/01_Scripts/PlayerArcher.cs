using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Image[] heartLife;
    public GameObject arco;

    public Image ability;
    private SpriteRenderer playerSprite;
    private BoxCollider2D playerCollider;

    public float abilityCooldown = 5f; // Tiempo en segundos que dura el cooldown
    bool isAbilityReady = true;
    float abilityTimer = 0;

    public float activeOpacity = 0.5f; // Opacidad del jugador cuando la habilidad está activa
    public float normalOpacity = 1f;
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerSprite = GetComponent<SpriteRenderer>();

        if (arco != null)
        {
            arco.SetActive(false);
        }
    }

    void Update()
    {
        HandleMovement();
        Shoot();
        Ability();
        AbilityCooldownTimer();
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
            // Ajustar la rotación del arco para que apunte en la misma dirección que el firePoint,
            // con un desplazamiento si es necesario (ajusta el ángulo según sea necesario).
            float offsetAngle = -135f; // Cambia este valor según la orientación inicial del arco.
            arco.transform.rotation = firePoint.rotation * Quaternion.Euler(0, 0, offsetAngle);

            // Activar el arco
            arco.SetActive(true);

            Bullet b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            b.damage = damage;
            b.speed = bulletspeed;

            // Desactivar el arco después de un corto tiempo
            StartCoroutine(HideArcoAfterDelay(0.5f));

            timer = 0;
        }
    }

    // Coroutine para desactivar el arco después de un retraso
    private IEnumerator HideArcoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        arco.SetActive(false);
    }
    void Ability()
    {
        // Si la habilidad está lista y se presiona el botón, activarla
        if (isAbilityReady && Input.GetKeyDown(KeyCode.R))
        {
            gameObject.tag = "Invisible"; // Cambia el tag a "Invisible"
            isAbilityReady = false; // Desactivar la habilidad y empezar el cooldown
            abilityTimer = abilityCooldown; // Reiniciar el temporizador del cooldown
            ability.fillAmount = 1;

            SetPlayerOpacity(activeOpacity);
        }
    }
    void AbilityCooldownTimer()
    {
        if (!isAbilityReady) // Si la habilidad no está lista, reducir el timer
        {
            abilityTimer -= Time.deltaTime;

            // Actualiza el fillAmount para que se reduzca gradualmente de 1 a 0 dependiendo del tiempo restante del cooldown
            ability.fillAmount = abilityTimer / abilityCooldown;

            if (abilityTimer <= 0)
            {
                isAbilityReady = true; // Habilidad lista cuando el cooldown termina
                ability.fillAmount = 0; // Habilidad lista, fillAmount es 0
                SetPlayerOpacity(normalOpacity);
                gameObject.tag = "Player";
            }
        }
    }
    void SetPlayerOpacity(float opacity)
    {
        Color color = playerSprite.color;
        color.a = opacity; // Cambiar el valor alfa del color (opacidad)
        playerSprite.color = color; // Aplicar el nuevo color con la opacidad
    }
}


