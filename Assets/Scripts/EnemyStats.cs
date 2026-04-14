using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Basic Stats")]
    public int maxHP = 5;
    public int currentHP;

    public float moveSpeed = 2f;
    public int attackDamage = 1;

    void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
    }
}