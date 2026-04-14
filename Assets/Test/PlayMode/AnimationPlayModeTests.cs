using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class AnimationPlayModeTests
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

// TC_ANIM_01 - Animation trúng đòn
[UnityTest]
    [System.Obsolete]
    public IEnumerator Test_Anim_TrungDon()
{
    player.TakeDamage(1);

    yield return new WaitForSeconds(0.5f);

    Animator anim = player.GetComponent<Animator>();

    Assert.IsNotNull(anim);
}

// TC_ANIM_02 - Animation chết
[UnityTest]
    [System.Obsolete]
    public IEnumerator Test_Anim_Chet()
{
    player.stats.currentHP = 1;

    player.TakeDamage(5);

    yield return new WaitForSeconds(1f);

    Assert.LessOrEqual(player.stats.currentHP, 0);
}

}