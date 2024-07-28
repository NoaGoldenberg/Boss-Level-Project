using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;
    private bool isPaused;

    [SerializeField] private AudioClip loseAudioClip;
    [SerializeField] private AudioClip WinAudioClip;
    [SerializeField] private AudioClip HitJellyfishAudioClip;
    [SerializeField] private AudioClip HitOctopusAudioClip;
    [SerializeField] private AudioClip crabAudioClip;
    [SerializeField] private AudioClip HitAttackAudioClip;
    
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayLoseSound() => PlaySound(loseAudioClip);
    public void PlayHitJellyfishSound() => PlaySound(HitJellyfishAudioClip);
    public void PlayCrabSound() => PlaySound(crabAudioClip);
    public void PlayHitOctopusSound() => PlaySound(HitOctopusAudioClip);
    public void PlayWinOctopusSound() => PlaySound(WinAudioClip);
    public void PlayHitAttackSound() => PlaySound(HitAttackAudioClip);
  

    public void PauseAudio()
    {
        Debug.Log("pause music");
        if (!audioSource.mute)
        {
            audioSource.Pause();
            isPaused = true;
        }
    }

    public void ResumeAudio()
    {
        if (isPaused)
        {
            audioSource.UnPause();
            isPaused = false;
        }
    }
    
    public void ToggleMute()
    {
        audioSource.mute = !audioSource.mute;
        isPaused = true;
    }
}
