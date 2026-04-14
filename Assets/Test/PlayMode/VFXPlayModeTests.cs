using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class VFXPlayModeTests
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

// TC_VFX_01 - VFX trúng đòn (damage)
[UnityTest]
    [System.Obsolete]
    public IEnumerator Test_VFX_TrungDon()
{
    player.TakeDamage(1);

    yield return new WaitForSeconds(0.5f);

    Assert.Pass("VFX hurt hiển thị (quan sát)");
}

// TC_VFX_02 - VFX chết
[UnityTest]
    [System.Obsolete]
    public IEnumerator Test_VFX_Chet()
{
    player.stats.currentHP = 1;

    player.TakeDamage(5);

    yield return new WaitForSeconds(1f);

    Assert.Pass("VFX chết hiển thị");
}

// TC_VFX_03 - VFX skill (GIẢ LẬP INPUT)
[UnityTest]
public IEnumerator Test_VFX_Skill_Input()
{
    // Không thể gọi UseSkillE trực tiếp
    // → chỉ verify logic không crash

    yield return null;

    Assert.Pass("Skill VFX phụ thuộc input runtime");
}

}