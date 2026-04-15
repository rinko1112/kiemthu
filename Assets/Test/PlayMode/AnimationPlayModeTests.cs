using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class AnimationPlayModeTests
{
    private PlayerController player;
    private Animator anim;

    [UnitySetUp]
    public IEnumerator KhoiTao()
    {
        if (SceneManager.GetActiveScene().name != "SampleScene")
            yield return SceneManager.LoadSceneAsync("SampleScene");

        yield return null;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        GameUIManager ui = Object.FindFirstObjectByType<GameUIManager>();
        if (ui != null)
        {
            if (ui.menuPanel != null) ui.menuPanel.SetActive(false);
            if (ui.pausePanel != null) ui.pausePanel.SetActive(false);
            ui.enabled = false;
        }

        player = Object.FindFirstObjectByType<PlayerController>();
        Assert.IsNotNull(player);

        anim = player.GetComponent<Animator>();
        Assert.IsNotNull(anim);

        yield return null;
    }

    // =========================
    // TC_ANIM_01 - Animation trúng đòn
    // =========================
    [UnityTest, Order(1)]
    public IEnumerator Test_Anim_TrungDon()
    {
        int hpCu = player.stats.currentHP;

        player.TakeDamage(1);

        yield return WaitForCondition(
            () => anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt"),
            2f
        );

        // 👉 cho bạn nhìn rõ
        yield return new WaitForSecondsRealtime(1.5f);

        Assert.Less(player.stats.currentHP, hpCu);
    }

    // =========================
    // TC_ANIM_02 - Animation chết
    // =========================
    [UnityTest, Order(2)]
    public IEnumerator Test_Anim_Chet()
    {
        player.stats.currentHP = 1;

        player.TakeDamage(5);

        yield return WaitForCondition(
            () => anim.GetCurrentAnimatorStateInfo(0).IsName("Die"),
            2f
        );

        // 👉 cho bạn nhìn rõ
        yield return new WaitForSecondsRealtime(2f);

        Assert.LessOrEqual(player.stats.currentHP, 0);
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
                Assert.Fail("Timeout - Animation không trigger");
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }
}