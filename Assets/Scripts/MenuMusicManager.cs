using UnityEngine;

public class MenuMusicManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Playlist")]
    public AudioClip[] musicList;

    public bool playOnStart = true;
    public bool isRandom = true;

    private int currentIndex = 0;

    void Start()
    {
        if (audioSource == null || musicList.Length == 0) return;

        if (playOnStart)
        {
            PlayMusic();
        }
    }

    void Update()
    {
        if (audioSource == null || musicList.Length == 0) return;

        // nếu nhạc kết thúc → chuyển bài
        if (!audioSource.isPlaying)
        {
            NextTrack();
        }
    }

    void PlayMusic()
    {
        if (isRandom)
        {
            currentIndex = Random.Range(0, musicList.Length);
        }
        else
        {
            currentIndex = 0;
        }

        audioSource.clip = musicList[currentIndex];
        audioSource.Play();
    }

    void NextTrack()
    {
        if (isRandom)
        {
            currentIndex = Random.Range(0, musicList.Length);
        }
        else
        {
            currentIndex++;
            if (currentIndex >= musicList.Length)
                currentIndex = 0;
        }

        audioSource.clip = musicList[currentIndex];
        audioSource.Play();
    }
}