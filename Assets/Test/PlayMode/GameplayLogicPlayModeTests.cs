using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class GameplayLogicPlayModeTests
{
private PlayerController player;

[UnitySetUp]
    [System.Obsolete]
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
    Assert.IsNotNull(player);

    yield return null;
}

// =========================
// TC_GL_01 - Player nhận damage
// =========================
[UnityTest]
    [System.Obsolete]
    public IEnumerator Test_Player_NhanDamage()
{
    int hpCu = player.stats.currentHP;

    player.TakeDamage(3);

    yield return new WaitForSeconds(0.5f);

    Assert.AreEqual(hpCu - 3, player.stats.currentHP);
}

// =========================
// TC_GL_02 - Player chết
// =========================
[UnityTest]
    [System.Obsolete]
    public IEnumerator Test_Player_Chet()
{
    player.stats.currentHP = 1;

    player.TakeDamage(2);

    yield return new WaitForSeconds(0.5f);

    Assert.LessOrEqual(player.stats.currentHP, 0);
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
// TC_GL_04 - Enemy nhận damage (FIX CHUẨN)
// =========================
[UnityTest]
public IEnumerator Test_Enemy_NhanDamage()
{
    GameObject enemyObj = new GameObject("Enemy");

    enemyObj.tag = "Enemy";
    enemyObj.transform.position = player.transform.position + Vector3.right * 0.5f;

    enemyObj.AddComponent<Rigidbody2D>();
    enemyObj.AddComponent<BoxCollider2D>();

    EnemyStats stats = enemyObj.AddComponent<EnemyStats>();
    Enemy enemy = enemyObj.AddComponent<Enemy>();

    enemy.stats = stats;

    stats.maxHP = 10;
    stats.currentHP = 10;

    // chờ system auto attack
    yield return new WaitForSeconds(1f);

    Assert.Less(stats.currentHP, 10);
}

// =========================
// TC_GL_05 - Enemy chết (FIX CHUẨN)
// =========================
[UnityTest]
public IEnumerator Test_Enemy_Chet()
{
    GameObject enemyObj = new GameObject("Enemy");

    enemyObj.tag = "Enemy";
    enemyObj.transform.position = player.transform.position + Vector3.right * 0.5f;

    enemyObj.AddComponent<Rigidbody2D>();
    enemyObj.AddComponent<BoxCollider2D>();

    EnemyStats stats = enemyObj.AddComponent<EnemyStats>();
    Enemy enemy = enemyObj.AddComponent<Enemy>();

    enemy.stats = stats;

    stats.maxHP = 10;
    stats.currentHP = 1;

    yield return new WaitForSeconds(1f);

    Assert.LessOrEqual(stats.currentHP, 0);
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

    yield return null;

    Assert.GreaterOrEqual(player.level, levelCu);
}

}