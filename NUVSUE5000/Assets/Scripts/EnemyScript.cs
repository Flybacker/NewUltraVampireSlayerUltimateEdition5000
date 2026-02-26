using System.Collections;
using UnityEngine;
public class EnemyScript : MonoBehaviour
{
    [SerializeField] Collider2D enemyCollider;
    [SerializeField] float speed;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Rigidbody2D enemyRB;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] ParticleSystem EnemyHitParticles;
    bool isAlive;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        isAlive = true;
    }

    void FixedUpdate()
    {
        if (isAlive)
        {
            enemyRB.linearVelocityX = speed;
            if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right * speed, 0.6f, layerMask))
            {
                speed = speed * -1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemyDamage") && isAlive)
        {
            isAlive = false;
            enemyRB.simulated = false;
            enemyCollider.enabled = false;
            audioManager.playSFX(audioManager.enemydeath);
            audioManager.playSFX(audioManager.explotion);
            transform.position = transform.position + Vector3.down * 0.75f ;
            transform.rotation = Quaternion.Euler(0,0,70);
            spriteRenderer.color = Color.red;
            EnemyHitParticles.Play();
            StartCoroutine(timer());
        }
    }
    IEnumerator timer()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

}
