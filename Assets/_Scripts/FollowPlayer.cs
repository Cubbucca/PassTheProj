using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform followTarget;
    [SerializeField]
    private Vector3 followOffset;
    [SerializeField, Range(.01f, 1f)]
    private float smoothSpeed = 0.125f;

    private Vector3 velocity;

    private void Update() {
        Vector3 desiredPosition = followTarget.position + followOffset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }
}
