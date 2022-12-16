using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage = 10;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _lifeTime;
    
    public void Initialize(Vector3 direction)
    {
        _rigidbody.velocity = direction * _speed;
        Destroy(gameObject, _lifeTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}