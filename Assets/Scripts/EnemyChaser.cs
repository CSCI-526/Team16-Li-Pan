using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChaser : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float attackRange = 0.7f;
    [SerializeField] private float attackCooldown = 0.8f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private int maxHealth = 3;

    [Header("Obstacle Avoidance")]
    [SerializeField] private float wallCheckDistance = 0.8f;
    [SerializeField] private float sideCheckDistance = 1.2f;
    [SerializeField] private float sideAngle = 60f;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D rb;
    private Transform player;
    private int currentHealth;
    private float nextAttackTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;

        if (distance <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;

            if (Time.time >= nextAttackTime)
            {
                PlayerHealth health = player.GetComponent<PlayerHealth>();

                if (health != null)
                    health.TakeDamage(attackDamage);

                nextAttackTime = Time.time + attackCooldown;
            }

            return;
        }

        Vector2 direction = toPlayer.normalized;

        // Check directly ahead.
        RaycastHit2D frontHit = Physics2D.Raycast(
            transform.position,
            direction,
            wallCheckDistance,
            wallLayer
        );

        if (frontHit.collider == null)
        {
            // Normal movement.
            rb.linearVelocity = direction * moveSpeed;
            return;
        }

        // Wall detected. Check two angled directions.
        Vector2 leftDirection = Rotate(direction, sideAngle);
        Vector2 rightDirection = Rotate(direction, -sideAngle);

        bool leftBlocked = Physics2D.Raycast(
            transform.position,
            leftDirection,
            sideCheckDistance,
            wallLayer
        );

        bool rightBlocked = Physics2D.Raycast(
            transform.position,
            rightDirection,
            sideCheckDistance,
            wallLayer
        );

        if (!leftBlocked && rightBlocked)
        {
            rb.linearVelocity = leftDirection * moveSpeed;
        }
        else if (leftBlocked && !rightBlocked)
        {
            rb.linearVelocity = rightDirection * moveSpeed;
        }
        else if (!leftBlocked && !rightBlocked)
        {
            // Both sides are open.
            // Choose the side that is more aligned with the player.
            float leftDot = Vector2.Dot(leftDirection, direction);
            float rightDot = Vector2.Dot(rightDirection, direction);

            rb.linearVelocity =
                (leftDot >= rightDot ? leftDirection : rightDirection)
                * moveSpeed;
        }
        else
        {
            // Both sides blocked.
            // Move along the wall.
            Vector2 wallNormal = frontHit.normal;

            Vector2 tangent = new Vector2(
                -wallNormal.y,
                wallNormal.x
            );

            if (Vector2.Dot(tangent, direction) < 0)
            {
                tangent = -tangent;
            }

            rb.linearVelocity = tangent * moveSpeed;
        }
    }

    private Vector2 Rotate(Vector2 vector, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;

        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        ).normalized;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}