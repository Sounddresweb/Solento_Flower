using UnityEngine;
using System.Collections; // ESENCIAL para las corrutinas

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))] // Nos aseguramos de que tenga SpriteRenderer
public class EnemigoBasico : MonoBehaviour
{
    public enum State { Idle, Chasing, Attacking }
    
    [Header("Estado Actual")]
    public State currentState = State.Idle;

    [Header("Configuración de IA")]
    public Transform player;
    public float moveSpeed = 3f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float keepChasingRange = 8f;
    
    [Header("Combate")]
    public int health = 3;

    [Header("Efecto de Retroceso (Knockback)")]
    public float knockbackForce = 7f;
    public float knockbackDuration = 0.2f;
    private bool isKnockback;

    [Header("Efecto Visual de Daño (Flash)")]
    public Color colorFlash = Color.white; // Color al que cambiará (blanco o rojo suelen funcionar bien)
    public float flashDuration = 0.1f;    // Qué tan rápido parpadea
    private Color colorOriginal;

    // Componentes
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveDirection;

    void Awake()
{
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    colorOriginal = spriteRenderer.color;

    if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;

    // --- NUEVO: Forzamos la persecución desde el inicio ---
    currentState = State.Chasing; 
}

    void Update()
    {
        if (player == null || isKnockback || health <= 0) return;
    
        // Esto te dirá en la consola si cambió a Idle por estar lejos
        Debug.Log("Estado actual: " + currentState + " | Distancia: " + Vector2.Distance(transform.position, player.position));
        // Si no hay jugador, o estamos en knockback, o estamos muertos, no calculamos IA
        if (player == null || isKnockback || health <= 0) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                if (distanceToPlayer < detectionRange) currentState = State.Chasing;
                moveDirection = Vector2.zero;
                break;

            case State.Chasing:
                 // Elimina o comenta esta línea para que nunca deje de perseguirte:
                // if (distanceToPlayer > keepChasingRange) currentState = State.Idle;
    
                 if (distanceToPlayer < attackRange) currentState = State.Attacking;
                 moveDirection = (player.position - transform.position).normalized;
                 break;

            case State.Attacking:
                if (distanceToPlayer > attackRange) currentState = State.Chasing;
                moveDirection = Vector2.zero;
                // Aquí iría la lógica de daño al jugador
                break;
        }
    }

    void FixedUpdate()
    {
        // 1. PRIMERO revisamos el retroceso.
        if (isKnockback || health <= 0) return;

        // 2. Si no hay retroceso, aplicamos movimiento de IA
        if (currentState == State.Chasing)
        {
            rb.linearVelocity = moveDirection * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Añade esto para depurar:
    Debug.Log("Detecté algo: " + collision.name); 

    if (collision.CompareTag("Bullet"))
    {
        // Evitamos recibir 
        // daño si ya estamos muertos o en proceso de morir
        if (health <= 0) return;

        if (collision.CompareTag("Bullet"))
        {
            Vector2 direccionImpacto = (transform.position - collision.transform.position).normalized;
            TomarDanio(1, direccionImpacto);
            
            // Devolvemos la bala al pool (la desactivamos)
            collision.gameObject.SetActive(false); 
        }
    }
    }

    public void TomarDanio(int cantidad, Vector2 direccion)
    {
        health -= cantidad;

        // Ejecutamos ambos efectos en paralelo
        StartCoroutine(KnockbackRoutine(direccion));
        StartCoroutine(FlashRoutine());

        Debug.Log("Enemigo golpeado. Vida: " + health);

        if (health <= 0)
        {
            Morir();
        }
    }

    // --- CORRUTINA DE FÍSICA ---
    private IEnumerator KnockbackRoutine(Vector2 direccion)
    {
        isKnockback = true;
        rb.linearVelocity = direccion * knockbackForce;
        
        yield return new WaitForSeconds(knockbackDuration);
        
        rb.linearVelocity = Vector2.zero;
        isKnockback = false;
    }

    // --- CORRUTINA VISUAL ---
    private IEnumerator FlashRoutine()
    {
        // Cambiamos al color de flash
        spriteRenderer.color = colorFlash;
        
        // Esperamos un tiempo muy corto
        yield return new WaitForSeconds(flashDuration);
        
        // Volvemos al color original
        spriteRenderer.color = colorOriginal;
    }

    void Morir()
    {
        Debug.Log("Enemigo Muerto");
        // Aquí podrías desactivar el colisionador para que no estorbe
        GetComponent<Collider2D>().enabled = false;
        
        // Opcional: Pequeña pausa antes de destruir para que se vea el último flash
        Destroy(gameObject, 0.1f); 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}