using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public EnemyStats stats;

    [Header("AI")]
    public float chaseRange = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;

    private float lastAttackTime;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    public HealthBar hpBar;

    [Header("Drop")]
    public GameObject expPrefab;

    // 🔥 HEAL DROP
    [Header("Heal Drop")]
    public GameObject healPrefab;
    [Range(0f, 1f)]
    public float healDropChance = 0.3f; // 30%

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (stats == null)
            stats = GetComponent<EnemyStats>();

        if (hpBar != null)
            hpBar.SetHP(stats.currentHP, stats.maxHP);

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    [System.Obsolete]
    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= chaseRange && distance > attackRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            rb.linearVelocity = new Vector2(direction.x * stats.moveSpeed, rb.linearVelocity.y);

            anim.SetBool("isRun", true);

            if (direction.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetBool("isRun", false);
        }

        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            anim.SetTrigger("attack");

            if (player != null)
            {
                PlayerController pc = player.GetComponent<PlayerController>();
                if (pc != null)
                {
                    pc.TakeDamage(stats.attackDamage);
                }
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        stats.TakeDamage(dmg);

        if (hpBar != null)
            hpBar.SetHP(stats.currentHP, stats.maxHP);

        anim.SetTrigger("hurt");

        if (stats.currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetTrigger("die");
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        // 🔥 SCORE
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(50);
        }

        // 🔥 EXP
        if (expPrefab != null)
        {
            Instantiate(expPrefab, transform.position, Quaternion.identity);
        }

        // 🔥 HEAL DROP (30%)
        if (healPrefab != null && Random.value < healDropChance)
        {
            Instantiate(healPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 1.5f);
    }
}