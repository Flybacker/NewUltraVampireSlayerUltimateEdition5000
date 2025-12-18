using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Collider2D DamageHitbox;

    SpriteRenderer sprite;

    [SerializeField] LayerMask layers;

    [SerializeField] ParticleSystem crashParticle;
    [SerializeField] ParticleSystem trailParticle;
    void Start()
    {
        DamageHitbox = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();

        gameObject.transform.Rotate(0, 0, Random.Range(-3f, 3f));
    }


    private void Update()
    {
        transform.position += transform.right * 25 * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            DamageHitbox.enabled = false;
            sprite.enabled = false;
            trailParticle.Stop();
            crashParticle.Play();
            StartCoroutine(timer());
        }

    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
