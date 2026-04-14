using UnityEngine;

public class HealItem : MonoBehaviour
{
    [Range(0f, 1f)]
    public float healPercent = 0.5f; // 🔥 50%

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerStats stats = collision.GetComponent<PlayerStats>();
        PlayerController pc = collision.GetComponent<PlayerController>();

        if (stats != null)
        {
            int healAmount = Mathf.CeilToInt(stats.maxHP * healPercent);
            stats.Heal(healAmount);

            // update UI HP
            if (pc != null && pc.hpBar != null)
            {
                pc.hpBar.SetHP(stats.currentHP, stats.maxHP);
            }
        }

        Destroy(gameObject);
    }
}