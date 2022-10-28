using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _minZoom = -11, _maxZoom = -16;
    [SerializeField] private float _maxCartVelocity = 10;
    private CinemachineTransposer _transponder;
    private ShoppingCart _cart;
    private float _vel;

    private void Start() {
        _transponder = _cam.GetCinemachineComponent<CinemachineTransposer>();
        _cart = FindObjectOfType<ShoppingCart>();
    }

    void LateUpdate() {
        var point = Mathf.InverseLerp(0, _maxCartVelocity, _cart.VelocityMagnitude);
        var targetZoom = Mathf.Lerp(_minZoom, _maxZoom, point);
        _transponder.m_FollowOffset = new Vector3(_transponder.m_FollowOffset.x, _transponder.m_FollowOffset.y, Mathf.SmoothDamp(_transponder.m_FollowOffset.z, targetZoom, ref _vel, _speed));
    }
}