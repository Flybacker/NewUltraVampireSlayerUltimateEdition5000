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
        enemyRB.linearVelocityX = speed;
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right * Mathf.Abs(speed), 0.6f, layerMask))
        {
            speed = speed * -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemyDamage")
        {
            
        }
    }
}
