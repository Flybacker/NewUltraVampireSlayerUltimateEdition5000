using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject enemy;
    float timer;
    void Start()
    {
        
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 3;
            Instantiate(enemy, transform.position, transform.rotation);
        }
    }
}
