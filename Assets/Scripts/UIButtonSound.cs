using UnityEngine;
using UnityEngine.UI;

public class UIButtonSound : MonoBehaviour
{
    public static UIButtonSound Instance;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    void Awake()
    {
        Instance = this;
    }

    [System.Obsolete]
    void Start()
    {
        SetupAllButtons();
    }

    [System.Obsolete]
    void SetupAllButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>(true); // 🔥 lấy cả button đang inactive

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(PlayClickSound);
        }
    }

    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}