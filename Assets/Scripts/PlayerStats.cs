using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Basic Stats")]
    public int maxHP = 10;
    public int currentHP;

    public float moveSpeed = 5f;
    public int attackDamage = 1;

    void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
    }

    public void Heal(int amount)
    {
        currentHP += amount;

        if (currentHP > maxHP)
            currentHP = maxHP;
    }
}