using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager Instance;

    [Header("UI")]
    public GameObject panel;

    private PlayerStats playerStats;
    private int selectedStat = -1;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenLevelUp(PlayerStats stats)
    {
        playerStats = stats;
        selectedStat = -1;

        if (panel != null)
            panel.SetActive(true);

        Time.timeScale = 0f; // ⛔ pause game
    }

    // ===== BUTTON CHỌN STAT =====
    public void SelectHP()
    {
        selectedStat = 0;
    }

    public void SelectSPD()
    {
        selectedStat = 1;
    }

    public void SelectATK()
    {
        selectedStat = 2;
    }

    // ===== CONFIRM =====
    public void Confirm()
    {
        if (playerStats == null || selectedStat == -1) return;

        switch (selectedStat)
        {
            case 0:
                playerStats.maxHP += 3;
                playerStats.currentHP += 3;
                break;

            case 1:
                playerStats.moveSpeed += 1f;
                break;

            case 2:
                playerStats.attackDamage += 3;
                break;
        }

        CloseLevelUp();
    }

    void CloseLevelUp()
    {
        if (panel != null)
            panel.SetActive(false);

        Time.timeScale = 1f; // ▶ resume game
    }
}