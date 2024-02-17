using System.Collections;

using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    public static MusicManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MusicManager>();

                if (instance == null)
                {
                    GameObject newMusicManager = new GameObject("MusicManager");
                    instance = newMusicManager.AddComponent<MusicManager>();
                }
            }

            return instance;
        }
    }

    private AudioSource musicSource;
    private float crossfadeDuration = 1.0f;
    private bool isCrossfading = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            musicSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayMusic(AudioClip musicClip, float volume = 1.0f)
    {
        if (!isCrossfading)
        {
            musicSource.volume = volume;
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            CrossfadeToNewTrack(musicClip, volume);
        }
    }

    public void CrossfadeToNewTrack(AudioClip newClip, float newVolume = 1.0f)
    {
        StartCoroutine(CrossfadeCoroutine(newClip, newVolume));
    }

    private IEnumerator CrossfadeCoroutine(AudioClip newClip, float newVolume)
    {
        isCrossfading = true;

        float startVolume = musicSource.volume;
        float startTime = Time.time;
        float endTime = startTime + crossfadeDuration;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / crossfadeDuration;
            musicSource.volume = Mathf.Lerp(startVolume, 0, t);
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = newVolume;
        musicSource.clip = newClip;
        musicSource.Play();

        isCrossfading = false;
    }
}