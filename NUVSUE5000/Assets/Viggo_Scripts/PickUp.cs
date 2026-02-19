using UnityEngine;

public class PickUp : MonoBehaviour
{

    [SerializeField] ParticleSystem PickUpParticles;
    
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            audioManager.playSFX(audioManager.Pickup);
            PickUpParticles.Play();
            Destroy(gameObject);
        }
    }
}
