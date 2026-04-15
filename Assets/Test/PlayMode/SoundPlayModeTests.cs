using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class SoundPlayModeTests
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
    // TC_SOUND_01 - Attack sound
    // =========================
    [UnityTest, Order(1)]
    public IEnumerator Test_AmThanh_Danh()
    {
        Assert.IsNotNull(player.audioSource);
        Assert.IsNotNull(player.attackSound);

        player.PlaySound(player.attackSound);

        // 👉 chỉ cần đảm bảo không crash + có clip
        yield return new WaitForSecondsRealtime(1f);

        Assert.Pass();
    }

    // =========================
    // TC_SOUND_02 - Skill sound
    // =========================
    [UnityTest, Order(2)]
    public IEnumerator Test_AmThanh_Skill()
    {
        Assert.IsNotNull(player.skillESound);

        player.PlaySound(player.skillESound);

        yield return new WaitForSecondsRealtime(1f);

        Assert.Pass();
    }

    // =========================
    // TC_SOUND_03 - Hurt sound
    // =========================
    [UnityTest, Order(3)]
    public IEnumerator Test_AmThanh_TrungDon()
    {
        Assert.IsNotNull(player.hurtSound);

        player.TakeDamage(1);

        yield return new WaitForSecondsRealtime(1f);

        Assert.Pass();
    }
}