using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;

    [Header("UI")]
    public TextMeshProUGUI scoreText;

    private void Awake()
{
    if (Instance == null)
        Instance = this;
    else
        Destroy(gameObject);
}

    public void AddScore(int amount)
    {
        score += amount;

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}