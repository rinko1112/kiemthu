using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public PlayerStats stats;

    [Header("Movement")]
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;

    private float moveInput;

    [Header("Attack")]
    public GameObject hitVFX;
    public float baseAttackCooldown = 2f;
    private float lastAttackTime;

    [Header("Skill E")]
    public GameObject skillVFX;
    public float skillRange = 2f;
    public int skillDamage = 3;
    public LayerMask enemyLayer;

    public float baseSkillCooldown = 5f;
    private float lastSkillETime;

    // ===== 🔥 AUDIO =====
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip skillESound;
    public AudioClip hurtSound;

    [Header("UI")]
    public HealthBar hpBar;

    [Header("Level System")]
    public int level = 1;
    public int maxLevel = 20;

    public int currentExp = 0;
    public int expToNextLevel = 100;

    public ExpBar expBar;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (stats == null)
            stats = GetComponent<PlayerStats>();

        if (hpBar != null)
            hpBar.SetHP(stats.currentHP, stats.maxHP);

        if (expBar != null)
        {
            expBar.SetExp(currentExp, expToNextLevel);
            expBar.SetLevel(level);
        }
    }

    void Update()
    {
        // ===== SKILL E =====
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseSkillE();
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetBool("isJump", true);
        }

        // ===== ATTACK =====
        float attackCooldown = GetAttackCooldown();

        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            anim.SetTrigger("attack");

            // 🔥 SOUND
            PlaySound(attackSound);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            GameObject nearestEnemy = null;
            float minDist = Mathf.Infinity;

            foreach (GameObject enemy in enemies)
            {
                float dist = Vector2.Distance(transform.position, enemy.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null)
            {
                Instantiate(hitVFX, nearestEnemy.transform.position, Quaternion.identity);

                Enemy enemyScript = nearestEnemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(stats.attackDamage);
                }
            }
        }

        anim.SetBool("isRun", moveInput != 0);

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * stats.moveSpeed, rb.linearVelocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (isGrounded && rb.linearVelocity.y <= 0)
        {
            anim.SetBool("isJump", false);
        }
    }

    // ===== SKILL E =====
    void UseSkillE()
    {
        float skillCooldown = GetSkillCooldown();

        if (Time.time < lastSkillETime + skillCooldown) return;

        lastSkillETime = Time.time;

        anim.SetTrigger("attack");

        // 🔥 SOUND
        PlaySound(skillESound);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject nearestEnemy = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            Vector3 hitPos = nearestEnemy.transform.position;

            Instantiate(skillVFX, hitPos, Quaternion.identity);

            Collider2D[] hits = Physics2D.OverlapCircleAll(hitPos, 5f, enemyLayer);

            foreach (Collider2D col in hits)
            {
                Enemy enemyScript = col.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(skillDamage);
                }
            }
        }
    }

    // ===== COOLDOWN SCALE =====
    float GetAttackCooldown()
    {
        float baseSpeed = 5f;
        float bonus = stats.moveSpeed - baseSpeed;

        float cooldown = baseAttackCooldown - bonus * 0.15f;
        return Mathf.Clamp(cooldown, 0.3f, baseAttackCooldown);
    }

    float GetSkillCooldown()
    {
        float baseSpeed = 5f;
        float bonus = stats.moveSpeed - baseSpeed;

        float cooldown = baseSkillCooldown - bonus * 0.25f;
        return Mathf.Clamp(cooldown, 1f, baseSkillCooldown);
    }

    [System.Obsolete]
    public void TakeDamage(int dmg)
    {
        stats.TakeDamage(dmg);

        if (hpBar != null)
            hpBar.SetHP(stats.currentHP, stats.maxHP);

        anim.SetTrigger("hurt");

        // 🔥 SOUND
        PlaySound(hurtSound);

        if (stats.currentHP <= 0)
        {
            Die();
        }
    }

    [System.Obsolete]
void Die()
{
    anim.SetTrigger("die");

    rb.linearVelocity = Vector2.zero;
    GetComponent<Collider2D>().enabled = false;

    this.enabled = false;

    // 🔥 CHỜ ANIMATION XONG
    StartCoroutine(HandleDeath());
}

    [System.Obsolete]
    IEnumerator HandleDeath()
{
    // 👉 thời gian animation chết (sửa theo animation của bạn)
    float dieAnimTime = 1.5f;

    yield return new WaitForSeconds(dieAnimTime);

    // 🔥 HIỆN LOSE SAU KHI ANIM XONG
    GameResultUI ui = FindObjectOfType<GameResultUI>();
    if (ui != null)
    {
        ui.ShowLose();
    }

    Destroy(gameObject);
}

    // ===== 🔥 AUDIO HELPER =====
    public  void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void AddExp(int amount)
    {
        if (level >= maxLevel)
        {
            currentExp = expToNextLevel;
            if (expBar != null)
                expBar.SetExp(currentExp, expToNextLevel);
            return;
        }

        currentExp += amount;

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
        }

        if (expBar != null)
            expBar.SetExp(currentExp, expToNextLevel);
    }

    void LevelUp()
    {
        level++;

        expToNextLevel += 50;

        Debug.Log("LEVEL UP: " + level);

        if (expBar != null)
        {
            expBar.SetLevel(level);
            expBar.ShowLevelUp();
        }

        if (LevelUpManager.Instance != null)
        {
            LevelUpManager.Instance.OpenLevelUp(stats);
        }
    }
}