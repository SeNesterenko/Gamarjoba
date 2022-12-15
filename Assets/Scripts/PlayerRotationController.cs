using UnityEngine;

public class PlayerRotationController : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 15;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        var step = _rotationSpeed * Time.fixedDeltaTime;
        var yawCamera = _mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,yawCamera,0), step);
    }
}