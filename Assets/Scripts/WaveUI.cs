using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI enemyCountText;

    public void SetWave(int wave)
    {
        if (waveText != null)
            waveText.text = "Wave " + wave;
    }

    public void SetCountdown(float time)
    {
        if (countdownText != null)
            countdownText.text = "Next Wave: " + Mathf.CeilToInt(time);
    }

    public void ClearCountdown()
    {
        if (countdownText != null)
            countdownText.text = "";
    }

    public void SetEnemyCount(int alive, int total)
    {
        if (enemyCountText != null)
            enemyCountText.text = alive + " / " + total;
    }
}