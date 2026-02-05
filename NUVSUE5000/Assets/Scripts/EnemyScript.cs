using UnityEngine;
public class EnemyScript : MonoBehaviour
{
    [SerializeField] Collider2D enemyCollider;
    [SerializeField] float speed;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Rigidbody2D enemyRB;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Debug.Log(speed);
        enemyRB.linearVelocityX = speed;
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right * speed, 0.6f, layerMask))
        {
            speed = speed * -1;
            Debug.Log("a");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemyDamage"))
        {
            Destroy(gameObject);
        }
    }
}
