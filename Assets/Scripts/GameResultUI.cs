using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResultUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    // ===== WIN =====
    public void ShowWin()
    {
        if (winPanel != null)
            winPanel.SetActive(true);

        Time.timeScale = 0;
    }

    // ===== LOSE =====
    public void ShowLose()
    {
        if (losePanel != null)
            losePanel.SetActive(true);

        Time.timeScale = 0;
    }

    // ===== MENU (RESTART GAME) =====
    public void BackToMenu()
    {
        Time.timeScale = 1;

        // 🔥 RESTART SCENE
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}