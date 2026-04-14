using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject pausePanel;
    public GameObject settingPanel;

    [Header("Audio")]
    public Slider menuVolumeSlider;
    public Slider pauseVolumeSlider;

    private bool isPaused = false;
    private bool isInMenu = true;
public static GameUIManager Instance;
    void Start()
    {
        menuPanel.SetActive(true);
        pausePanel.SetActive(false);

        if (settingPanel != null)
            settingPanel.SetActive(false);

        Time.timeScale = 0;

        float volume = PlayerPrefs.GetFloat("volume", 1f);
        AudioListener.volume = volume;

        // 🔥 GÁN CHO MENU
        if (menuVolumeSlider != null)
        {
            menuVolumeSlider.value = volume;
            menuVolumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // 🔥 GÁN CHO PAUSE
        if (pauseVolumeSlider != null)
        {
            pauseVolumeSlider.value = volume;
            pauseVolumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isInMenu)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PlayGame()
    {
        isInMenu = false;
        isPaused = false;

        menuPanel.SetActive(false);
        pausePanel.SetActive(false);

        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        if (isInMenu) return;

        isPaused = true;
        pausePanel.SetActive(true);

        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);

        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        isInMenu = true;
        isPaused = false;

        menuPanel.SetActive(true);
        pausePanel.SetActive(false);

        Time.timeScale = 0;
    }

    public void OpenSetting()
    {
        if (settingPanel != null)
            settingPanel.SetActive(true);
    }

    public void CloseSetting()
    {
        if (settingPanel != null)
            settingPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // ===== VOLUME CHUNG =====
    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("volume", value);

        // 🔥 ĐỒNG BỘ 2 SLIDER
        if (menuVolumeSlider != null && menuVolumeSlider.value != value)
            menuVolumeSlider.value = value;

        if (pauseVolumeSlider != null && pauseVolumeSlider.value != value)
            pauseVolumeSlider.value = value;
    }
    void Awake()
{
    Instance = this;
}
}