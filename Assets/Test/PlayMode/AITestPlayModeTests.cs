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
    [System.Obsolete]
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

    // LẤY ENEMY (nếu có)
    enemy = Object.FindFirstObjectByType<Enemy>();

    // Nếu không có → tự tạo
    if (enemy == null)
    {
        GameObject enemyObj = new GameObject("Enemy");

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

    yield return new WaitForSeconds(1f);

    Vector3 viTriMoi = enemy.transform.position;

    Assert.AreNotEqual(viTriCu, viTriMoi);
}

// =========================
// TC_AI_02 - Enemy đứng yên khi xa
// =========================
[UnityTest]
public IEnumerator Test_Enemy_KhongTanCong_KhiXa()
{
    enemy.transform.position = player.transform.position + new Vector3(50, 0, 0);

    player.stats.currentHP = 10;

    yield return new WaitForSeconds(1.5f);

    Assert.AreEqual(10, player.stats.currentHP);
}

// =========================
// TC_AI_03 - Enemy tấn công
// =========================
[UnityTest]
public IEnumerator Test_Enemy_TanCong()
{
    player.stats.currentHP = 10;

    enemy.transform.position = player.transform.position + new Vector3(0.5f, 0, 0);

    yield return new WaitForSeconds(1.5f);

    Assert.Less(player.stats.currentHP, 10);
}

// =========================
// TC_AI_04 - Cooldown attack
// =========================
[UnityTest]
public IEnumerator Test_Enemy_KhongSpamAttack()
{
    player.stats.currentHP = 10;

    enemy.transform.position = player.transform.position + new Vector3(0.5f, 0, 0);

    yield return new WaitForSeconds(0.5f);

    int hp1 = player.stats.currentHP;

    yield return new WaitForSeconds(0.2f);

    int hp2 = player.stats.currentHP;

    // nếu cooldown đúng → không giảm liên tục
    Assert.GreaterOrEqual(hp2, hp1 - 1);
}

// =========================
// TC_AI_05 - Flip hướng
// =========================
[UnityTest]
public IEnumerator Test_Enemy_FlipHuong()
{
    player.transform.position = enemy.transform.position + Vector3.left * 2;

    yield return new WaitForSeconds(1f);

    float scaleX = enemy.transform.localScale.x;

    Assert.Less(scaleX, 0);
}

}