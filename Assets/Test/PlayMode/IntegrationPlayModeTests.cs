using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class IntegrationPlayModeTests
{
    private PlayerController player;

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

        yield return null;
    }

    // =========================
    // TC_INT_01 - Kill enemy (flow thật)
    // =========================
    [UnityTest]
    public IEnumerator Test_KillEnemy()
    {
        GameObject enemyPrefab = Resources.Load<GameObject>("Enemy");
        Assert.IsNotNull(enemyPrefab, "Không tìm thấy Enemy prefab");

        GameObject enemyObj = Object.Instantiate(
            enemyPrefab,
            player.transform.position + Vector3.right * 1f,
            Quaternion.identity
        );

        Enemy enemy = enemyObj.GetComponent<Enemy>();
        Assert.IsNotNull(enemy);

        yield return null; // 👉 đợi Start()

        enemy.stats.currentHP = 2;

        // 👉 giả lập combat thật
        enemy.TakeDamage(2);

        // chờ chết (HP <= 0)
        yield return WaitForCondition(
            () => enemy != null && enemy.stats.currentHP <= 0,
            2f
        );

        // chờ destroy (animation delay)
        yield return WaitForCondition(
            () => enemy == null,
            3f
        );

        Assert.Pass();
    }

    // =========================
    // TC_INT_02 - LevelUp flow thật
    // =========================
    [UnityTest]
    public IEnumerator Test_LevelUp_Flow()
    {
        int oldLevel = player.level;

        player.currentExp = player.expToNextLevel;

        player.AddExp(0);

        yield return WaitForCondition(
            () => player.level > oldLevel,
            3f
        );

        Assert.Greater(player.level, oldLevel);
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
                Assert.Fail("Timeout - Condition không đạt");
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }
}