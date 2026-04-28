using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Configuración de Evasión (Dash)")]
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDashing;
    private bool canDash = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDashing) return; // Bloquea input durante el dash

        // Captura de entrada (Normalizada para velocidad uniforme)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        // Gatillo de Evasión
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(EjecutarDash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        // Aplicación de física profesional
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private IEnumerator EjecutarDash()
    {
        canDash = false;
        isDashing = true;

        // Aplicamos el impulso de evasión
        rb.linearVelocity = moveInput * dashForce;

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}