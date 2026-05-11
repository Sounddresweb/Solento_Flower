using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float stopDistance = 0.6f;

    private Rigidbody2D rb;
    private Transform target;
    private bool playerDetected = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (playerDetected && target != null)
        {
            float distance = Vector2.Distance(transform.position, target.position);

            if (distance > stopDistance)
            {
                Vector2 direction = (target.position - transform.position).normalized;
                rb.linearVelocity = direction * moveSpeed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void DetectPlayer(Transform player)
    {
        target = player;
        playerDetected = true;
    }

    public void LosePlayer()
    {
        target = null;
        playerDetected = false;
    }
}
