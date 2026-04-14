using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBar : MonoBehaviour
{
    [Header("EXP Bar")]
    public Slider slider;
    public float smoothSpeed = 10f;

    private float targetValue;

    [Header("Level UI")]
    public TextMeshProUGUI levelText;

    [Header("Level Up Effect")]
    public GameObject levelUpTextObj;
    public float showTime = 1.5f;

    private Coroutine levelUpRoutine;

    // ===== SET EXP =====
    public void SetExp(float current, float max)
    {
        targetValue = current / max;
    }

    // ===== SET LEVEL =====
    public void SetLevel(int level)
    {
        if (levelText != null)
            levelText.text = "" + level;
    }

    // ===== LEVEL UP EFFECT =====
    public void ShowLevelUp()
    {
        if (levelUpTextObj == null) return;

        if (levelUpRoutine != null)
            StopCoroutine(levelUpRoutine);

        levelUpRoutine = StartCoroutine(LevelUpRoutine());
    }

    System.Collections.IEnumerator LevelUpRoutine()
    {
        levelUpTextObj.SetActive(true);

        yield return new WaitForSeconds(showTime);

        levelUpTextObj.SetActive(false);
    }

    void Update()
    {
        if (slider != null)
        {
            slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * smoothSpeed);
        }
    }
}