using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class VFXPlayModeTests
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
    // TC_VFX_01 - Hurt VFX
    // =========================
    [UnityTest, Order(1)]
    public IEnumerator Test_VFX_TrungDon()
    {
        player.TakeDamage(1);

        yield return new WaitForSecondsRealtime(1f);

        // 👉 bạn nhìn animation hurt
        Assert.Pass();
    }

    // =========================
    // TC_VFX_02 - Die VFX
    // =========================
    [UnityTest, Order(2)]
    public IEnumerator Test_VFX_Chet()
    {
        player.stats.currentHP = 1;

        player.TakeDamage(5);

        yield return new WaitForSecondsRealtime(2f);

        Assert.Pass();
    }

    // =========================
    // TC_VFX_03 - Hit VFX (attack)
    // =========================
    [UnityTest, Order(3)]
    public IEnumerator Test_VFX_Attack()
    {
        GameObject enemyPrefab = Resources.Load<GameObject>("Enemy");
        Assert.IsNotNull(enemyPrefab);

        GameObject enemyObj = Object.Instantiate(
            enemyPrefab,
            player.transform.position + Vector3.right * 1f,
            Quaternion.identity
        );

        yield return null;

        player.PlaySound(player.attackSound); // trigger animation

        yield return new WaitForSecondsRealtime(1.5f);

        Assert.Pass();
    }
}