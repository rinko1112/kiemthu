using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class SoundPlayModeTests
{
    private PlayerController player;

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

        player = Object.FindFirstObjectByType<PlayerController>();
        Assert.IsNotNull(player);

        yield return null;
    }

    [UnityTest]
    public IEnumerator Test_AmThanh_Danh()
    {
        player.PlaySound(player.attackSound);

        yield return new WaitForSeconds(0.3f);

        Assert.IsTrue(player.audioSource.isPlaying);
    }

    [UnityTest]
    public IEnumerator Test_AmThanh_Skill()
    {
        player.PlaySound(player.skillESound);

        yield return new WaitForSeconds(0.3f);

        Assert.IsTrue(player.audioSource.isPlaying);
    }
    [UnityTest]
    [System.Obsolete]
    public IEnumerator Test_AmThanh_TrungDon()
{
    player.TakeDamage(1);

    yield return new WaitForSeconds(0.3f);

    Assert.IsTrue(player.audioSource.isPlaying);
}
}