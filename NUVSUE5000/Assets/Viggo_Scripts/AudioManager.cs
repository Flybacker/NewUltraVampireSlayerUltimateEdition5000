using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource Ambiance;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip ambiance;
    public AudioClip Jump;
    public AudioClip Dash;
    public AudioClip Shot;
    public AudioClip Pickup;
    public AudioClip enemydeath;
    public AudioClip explotion;

    private void Start()
    {
        musicSource.clip = background;
        Ambiance.clip = ambiance;
        musicSource.Play();
        Ambiance.Play();
    }

    public void playSFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    //Copy this into the script where it's going to play the sound

    //Cach this: AudioManager audioManager;

    //private void Awake()
    //{
    //    audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    //}

    //Where the action and sound is going to be: audioManager.playSFX(audioManager.nameOfTheSound);
}
