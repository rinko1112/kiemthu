using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.Reflection;

public class IntegrationPlayModeTests
{
    private PlayerController player;
    private Enemy enemy;

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

        // ❌ KHÔNG disable LevelUpManager nữa
        // để auto chọn stat chạy đúng

        player = Object.FindFirstObjectByType<PlayerController>();
        enemy = Object.FindFirstObjectByType<Enemy>();

        Assert.IsNotNull(player);
        Assert.IsNotNull(enemy);

        yield return null;
    }

    // =========================
    // TC_INT_01 - Kill enemy (combat thật)
    // =========================
    [UnityTest]
    public IEnumerator Test_KillEnemy()
    {
        enemy.transform.position = player.transform.position + Vector3.right * 0.5f;

        enemy.stats.currentHP = 1;

        var method = typeof(PlayerController)
            .GetMethod("Attack", BindingFlags.NonPublic | BindingFlags.Instance);

        if (method == null)
        {
            Assert.Fail("Không tìm thấy Attack()");
        }

        method.Invoke(player, null);

        yield return new WaitForSeconds(0.5f);

        Assert.LessOrEqual(enemy.stats.currentHP, 0);
    }

    // =========================
    // TC_INT_02 - LevelUp flow (FIX CHUẨN)
    // =========================
    [UnityTest]
    public IEnumerator Test_LevelUp_Flow()
    {
        int levelCu = player.level;

        player.currentExp = player.expToNextLevel;
        player.AddExp(0);

        // 🔥 cho hệ thống auto chọn stat chạy
        yield return new WaitForSeconds(1f);

        Assert.Greater(player.level, levelCu);
    }
}