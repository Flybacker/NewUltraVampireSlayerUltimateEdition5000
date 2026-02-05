using Unity.VisualScripting;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] Transform cameraTarget;

    [SerializeField] float speed;
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(cameraTarget.position.x, cameraTarget.position.y, transform.position.z), speed * Time.deltaTime);
    }
}
