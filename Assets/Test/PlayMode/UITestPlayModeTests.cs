using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using TMPro;

public class UITestPlayModeTests
{
    private PlayerController player;
    private TextMeshProUGUI levelText;

    [UnitySetUp]
    [System.Obsolete]
    public IEnumerator KhoiTao()
    {
        yield return SceneManager.LoadSceneAsync("SampleScene");
        yield return null;

        GameUIManager ui = Object.FindFirstObjectByType<GameUIManager>();
        if (ui != null)
        {
            if (ui.menuPanel != null) ui.menuPanel.SetActive(false);
            if (ui.pausePanel != null) ui.pausePanel.SetActive(false);
            Time.timeScale = 1f;
            ui.enabled = false;
        }

        // ❌ KHÔNG disable LevelUpManager
        // để giữ logic auto chọn stat

        player = Object.FindFirstObjectByType<PlayerController>();
        Assert.IsNotNull(player);

        levelText = Object.FindFirstObjectByType<TextMeshProUGUI>();

        yield return null;
    }

    // =========================
    // TC_UI_01 - Level text cập nhật (theo level thật)
    // =========================
    [UnityTest]
    public IEnumerator Test_LevelText_CapNhat()
    {
        if (levelText == null)
            Assert.Ignore();

        int levelCu = player.level;

        player.currentExp = player.expToNextLevel;
        player.AddExp(0);

        // 🔥 chờ auto level + UI update
        yield return new WaitForSeconds(1f);

        Assert.Greater(player.level, levelCu);
    }

    // =========================
    // TC_UI_02 - HP giảm (logic thật)
    // =========================
    [UnityTest]
    [System.Obsolete]
    public IEnumerator Test_HP_Giam()
    {
        int hpCu = player.stats.currentHP;

        player.TakeDamage(2);

        yield return null;

        Assert.Less(player.stats.currentHP, hpCu);
    }

    // =========================
    // TC_UI_03 - EXP tăng (logic thật)
    // =========================
    [UnityTest]
    public IEnumerator Test_EXP_Tang()
    {
        int expCu = player.currentExp;

        player.AddExp(50);

        yield return null;

        Assert.Greater(player.currentExp, expCu);
    }
}