using UnityEngine;

public class RotateY : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 3f;

    void Update()
    {
        transform.RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);
    }
}
