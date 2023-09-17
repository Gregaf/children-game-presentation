using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();

                if (instance == null)
                {
                    GameObject newAudioManager = new GameObject("AudioManager");
                    instance = newAudioManager.AddComponent<AudioManager>();
                }
            }

            return instance;
        }
    }

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Play a sound
    public void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    // Stop all audio
    public void StopAll()
    {
        audioSource.Stop();
    }
}
