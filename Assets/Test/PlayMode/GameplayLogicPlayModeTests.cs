using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class GameplayLogicPlayModeTests
{
    private PlayerController player;

    [Header("Prefab")]
    public GameObject enemyPrefab;

    [UnitySetUp]
    public IEnumerator KhoiTao()
    {
        yield return SceneManager.LoadSceneAsync("SampleScene");
        yield return null;

        GameUIManager ui = GameObject.FindObjectOfType<GameUIManager>();
        if (ui != null)
        {
            if (ui.menuPanel != null) ui.menuPanel.SetActive(false);
            if (ui.pausePanel != null) ui.pausePanel.SetActive(false);
            Time.timeScale = 1f;
            ui.enabled = false;
        }

        player = GameObject.FindObjectOfType<PlayerController>();
        Assert.IsNotNull(player, "Không tìm thấy Player");

        yield return null;
    }

    // =========================
    // TC_GL_01 - Player nhận damage
    // =========================
    [UnityTest]
    public IEnumerator Test_Player_NhanDamage()
    {
        int hpCu = player.stats.currentHP;

        player.TakeDamage(3);

        yield return WaitForCondition(
            () => player.stats.currentHP == hpCu - 3,
            2f
        );
    }

    // =========================
    // TC_GL_02 - Player chết
    // =========================
    [UnityTest]
    public IEnumerator Test_Player_Chet()
    {
        player.stats.currentHP = 1;

        player.TakeDamage(2);

        yield return WaitForCondition(
            () => player.stats.currentHP <= 0,
            2f
        );
    }

    // =========================
    // TC_GL_03 - Hồi máu
    // =========================
    [UnityTest]
    public IEnumerator Test_Player_HoiMau()
    {
        player.stats.currentHP = 5;

        player.stats.Heal(10);

        yield return null;

        Assert.AreEqual(player.stats.maxHP, player.stats.currentHP);
    }

    // =========================
    // TC_GL_04 - Enemy nhận damage (DÙNG PREFAB)
    // =========================
    [UnityTest]
public IEnumerator Test_Enemy_NhanDamage()
{
    GameObject enemyPrefab = Resources.Load<GameObject>("Enemy");
    Assert.IsNotNull(enemyPrefab);

    GameObject enemyObj = Object.Instantiate(
        enemyPrefab,
        player.transform.position + Vector3.right * 1f,
        Quaternion.identity
    );

    Enemy enemy = enemyObj.GetComponent<Enemy>();
    Assert.IsNotNull(enemy);

    yield return null; // 👉 ĐỢI Start()

    int hpCu = enemy.stats.currentHP;

    enemy.TakeDamage(2);

    yield return WaitForCondition(
        () => enemy != null && enemy.stats.currentHP < hpCu,
        2f
    );

    Assert.Less(enemy.stats.currentHP, hpCu);

    Object.Destroy(enemyObj);
}

    // =========================
    // TC_GL_05 - Enemy chết (DÙNG PREFAB)
    // =========================
    [UnityTest]
public IEnumerator Test_Enemy_Chet()
{
    GameObject enemyPrefab = Resources.Load<GameObject>("Enemy");
    Assert.IsNotNull(enemyPrefab);

    GameObject enemyObj = Object.Instantiate(
        enemyPrefab,
        player.transform.position + Vector3.right * 1f,
        Quaternion.identity
    );

    Enemy enemy = enemyObj.GetComponent<Enemy>();
    Assert.IsNotNull(enemy);

    yield return null; // 👉 ĐỢI Start()

    enemy.stats.currentHP = 1;

    enemy.TakeDamage(5);

    // 👉 CHỜ HP <= 0 (logic chết)
    yield return WaitForCondition(
        () => enemy != null && enemy.stats.currentHP <= 0,
        2f
    );

    // 👉 CHỜ object bị destroy (animation + delay)
    yield return WaitForCondition(
        () => enemy == null,
        3f
    );

    Assert.Pass();
}

    // =========================
    // TC_GL_06 - Nhận EXP
    // =========================
    [UnityTest]
    public IEnumerator Test_Nhan_EXP()
    {
        int expCu = player.currentExp;

        player.AddExp(50);

        yield return null;

        Assert.Greater(player.currentExp, expCu);
    }

    // =========================
    // TC_GL_07 - Level Up
    // =========================
    [UnityTest]
    public IEnumerator Test_LevelUp()
    {
        int levelCu = player.level;

        player.currentExp = player.expToNextLevel;

        player.AddExp(0);

        yield return WaitForCondition(
            () => player.level > levelCu,
            2f
        );
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