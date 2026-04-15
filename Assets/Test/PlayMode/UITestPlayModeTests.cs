using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using TMPro;

public class UITestPlayModeTests
{
    private PlayerController player;
    private ExpBar expBar;

    [UnitySetUp]
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

        player = Object.FindFirstObjectByType<PlayerController>();
        Assert.IsNotNull(player);

        expBar = player.expBar;
        Assert.IsNotNull(expBar);

        yield return null;
    }

    // =========================
    // TC_UI_01 - Level text cập nhật
    // =========================
    [UnityTest, Order(1)]
    public IEnumerator Test_LevelText_CapNhat()
    {
        int levelCu = player.level;

        player.currentExp = player.expToNextLevel;
        player.AddExp(0);

        yield return WaitForCondition(
            () => player.level > levelCu,
            2f
        );

        // 👉 cho bạn nhìn UI
        yield return new WaitForSecondsRealtime(2f);

        Assert.Greater(player.level, levelCu);
    }

    // =========================
    // TC_UI_02 - HP giảm
    // =========================
    [UnityTest, Order(2)]
    public IEnumerator Test_HP_Giam()
    {
        int hpCu = player.stats.currentHP;

        player.TakeDamage(2);

        yield return WaitForCondition(
            () => player.stats.currentHP < hpCu,
            2f
        );

        yield return new WaitForSecondsRealtime(2f);

        Assert.Less(player.stats.currentHP, hpCu);
    }

    // =========================
    // TC_UI_03 - EXP tăng
    // =========================
    [UnityTest, Order(3)]
    public IEnumerator Test_EXP_Tang()
    {
        int expCu = player.currentExp;

        player.AddExp(50);

        yield return WaitForCondition(
            () => player.currentExp > expCu,
            2f
        );

        yield return new WaitForSecondsRealtime(2f);

        Assert.Greater(player.currentExp, expCu);
    }

    // =========================
    // HELPER
    // =========================
    protected IEnumerator WaitForCondition(System.Func<bool> condition, float timeout = 3f)
    {
        float timer = 0f;

        while (!condition())
        {
            if (timer >= timeout)
            {
                Assert.Fail("Timeout - UI không update");
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }
}