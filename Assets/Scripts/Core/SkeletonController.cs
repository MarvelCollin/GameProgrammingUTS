using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SkeletonAnimator skeletonAnimator;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private PlayerController targetPlayer;
    private Vector3 spawnPosition;

    private float detectionRange = 5f;
    private float chaseSpeed = 2f;
    private float attackRange = 1f;
    private float attackCooldown = 1f;
    private float lastAttackTime;

    private int maxHealth = 3;
    private int currentHealth;
    private bool isDead;
    private bool isHurt;
    private float hurtDuration = 0.5f;
    private float hurtTimer;

    private float respawnTime = 10f;
    private float respawnTimer;

    private float recoilForce = 5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
        }
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector2(0.16f, 0.16f);

        skeletonAnimator = new SkeletonAnimator(spriteRenderer);
    }

    private void Start()
    {
        spawnPosition = transform.position;
        currentHealth = maxHealth;
        isDead = false;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            targetPlayer = playerObj.GetComponent<PlayerController>();
        }

        skeletonAnimator.PlayIdle();
    }

    private void Update()
    {
        if (isDead)
        {
            HandleRespawn();
            return;
        }

        if (isHurt)
        {
            HandleHurt();
            return;
        }

        skeletonAnimator.Update();

        if (targetPlayer == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, targetPlayer.transform.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer <= attackRange)
            {
                TryAttack();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            skeletonAnimator.PlayIdle();
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = (targetPlayer.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;

        spriteRenderer.flipX = direction.x < 0;
        skeletonAnimator.PlayWalk();
    }

    private void TryAttack()
    {
        rb.linearVelocity = Vector2.zero;

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            skeletonAnimator.PlayAttack();

            Vector2 attackDirection = (targetPlayer.transform.position - transform.position).normalized;
            targetPlayer.TakeDamage(attackDirection * recoilForce);
        }
    }

    public void TakeDamage(Vector2 attackDirection)
    {
        if (isDead || isHurt) return;

        currentHealth--;
        isHurt = true;
        hurtTimer = hurtDuration;

        rb.linearVelocity = attackDirection.normalized * recoilForce;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            skeletonAnimator.PlayHurt();
            AudioManager.Instance.PlaySFX("hurt");
        }
    }

    private void HandleHurt()
    {
        hurtTimer -= Time.deltaTime;
        skeletonAnimator.Update();

        if (hurtTimer <= 0)
        {
            isHurt = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Die()
    {
        isDead = true;
        isHurt = false;
        respawnTimer = respawnTime;
        rb.linearVelocity = Vector2.zero;
        boxCollider.enabled = false;
        skeletonAnimator.PlayDeath();
        AudioManager.Instance.PlaySFX("death");
    }

    private void HandleRespawn()
    {
        skeletonAnimator.Update();
        respawnTimer -= Time.deltaTime;

        if (respawnTimer <= 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = spawnPosition;
        currentHealth = maxHealth;
        isDead = false;
        boxCollider.enabled = true;
        skeletonAnimator.PlayIdle();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead || isHurt) return;

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (player.IsAttacking())
                {
                    Vector2 attackDir = (transform.position - player.transform.position).normalized;
                    TakeDamage(attackDir);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isDead || isHurt) return;

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && player.IsAttacking())
            {
                Vector2 attackDir = (transform.position - player.transform.position).normalized;
                TakeDamage(attackDir);
            }
        }
    }
}
