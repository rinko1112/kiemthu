using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class AITestPlayModeTests
{
    private PlayerController player;
    private Enemy enemy;

    [UnitySetUp]
    public IEnumerator KhoiTao()
    {
        yield return SceneManager.LoadSceneAsync("SampleScene");
        yield return null;

        // TẮT UI MENU
        GameUIManager ui = Object.FindFirstObjectByType<GameUIManager>();
        if (ui != null)
        {
            if (ui.menuPanel != null)
                ui.menuPanel.SetActive(false);

            if (ui.pausePanel != null)
                ui.pausePanel.SetActive(false);

            Time.timeScale = 1f;
            ui.enabled = false;
        }

        // LẤY PLAYER
        player = Object.FindFirstObjectByType<PlayerController>();
        Assert.IsNotNull(player, "Không tìm thấy Player");

        // LẤY ENEMY
        enemy = Object.FindFirstObjectByType<Enemy>();

        if (enemy == null)
        {
            GameObject enemyObj = new GameObject("Enemy");

            enemyObj.tag = "Enemy";
            enemyObj.transform.position = player.transform.position + new Vector3(5, 0, 0);

            enemyObj.AddComponent<Rigidbody2D>();
            enemyObj.AddComponent<BoxCollider2D>();

            var anim = enemyObj.AddComponent<Animator>();
            anim.enabled = false;

            EnemyStats stats = enemyObj.AddComponent<EnemyStats>();
            enemy = enemyObj.AddComponent<Enemy>();

            enemy.stats = stats;

            stats.maxHP = 10;
            stats.currentHP = 10;
        }

        yield return null;
    }

    // =========================
    // TC_AI_01 - Enemy đuổi player
    // =========================
    [UnityTest]
    public IEnumerator Test_Enemy_Chase_Player()
    {
        Vector3 viTriCu = enemy.transform.position;

        yield return WaitForCondition(
            () => Vector3.Distance(enemy.transform.position, viTriCu) > 0.1f,
            3f
        );

        Assert.Pass();
    }

    // =========================
    // TC_AI_02 - Enemy không attack khi xa
    // =========================
    [UnityTest]
    public IEnumerator Test_Enemy_KhongTanCong_KhiXa()
    {
        enemy.transform.position = player.transform.position + new Vector3(50, 0, 0);

        player.stats.currentHP = 10;

        yield return new WaitForSeconds(1f);

        Assert.AreEqual(10, player.stats.currentHP);
    }

    // =========================
    // TC_AI_03 - Enemy attack
    // =========================
    [UnityTest]
    public IEnumerator Test_Enemy_TanCong()
    {
        player.stats.currentHP = 10;

        enemy.transform.position = player.transform.position + new Vector3(0.5f, 0, 0);

        yield return WaitForCondition(
            () => player.stats.currentHP < 10,
            3f
        );

        Assert.Pass();
    }

    // =========================
    // TC_AI_04 - Cooldown attack
    // =========================
    [UnityTest]
    public IEnumerator Test_Enemy_KhongSpamAttack()
    {
        player.stats.currentHP = 10;

        enemy.transform.position = player.transform.position + new Vector3(0.5f, 0, 0);

        yield return WaitForCondition(
            () => player.stats.currentHP < 10,
            3f
        );

        int hp1 = player.stats.currentHP;

        yield return new WaitForSeconds(0.3f);

        int hp2 = player.stats.currentHP;

        Assert.GreaterOrEqual(hp2, hp1 - 1);
    }

    // =========================
    // TC_AI_05 - Flip hướng
    // =========================
    [UnityTest]
    public IEnumerator Test_Enemy_FlipHuong()
    {
        player.transform.position = enemy.transform.position + Vector3.left * 2;

        yield return WaitForCondition(
            () => enemy.transform.localScale.x < 0,
            3f
        );

        Assert.Pass();
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