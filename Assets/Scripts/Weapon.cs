using UnityEngine;

[RequireComponent(typeof(WeaponEffectController))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _muzzle;
    [SerializeField] private float _rateShootPerSecond = 0.04f;
    [SerializeField] private Bullet _bullet;
    
    private float _accumulatedTime;
    private float _timeForOneShot;
    public bool IsShooting { get; private set; }
    
    private Ray _ray;

    private WeaponEffectController _weaponEffectController;

    private void Start()
    {
        _weaponEffectController = GetComponent<WeaponEffectController>();
        _timeForOneShot = 1f / _rateShootPerSecond;
    }

    public void StartShooting()
    {
        IsShooting = true;
        Shoot();
    }

    public void UpdateShooting(float deltaTime)
    {
        _accumulatedTime += deltaTime;

        while (_accumulatedTime >= 0)
        {
            Shoot();
            _accumulatedTime -= _timeForOneShot;
        }
    }

    public void StopShooting()
    {
        IsShooting = false;
    }

    private void Shoot()
    {
        _weaponEffectController.PlayShootEffect();

        _ray.origin = _muzzle.position;
        _ray.direction = _muzzle.forward;

        if (Physics.Raycast(_ray, out var hitInfo))
        {
            _weaponEffectController.PlayHitEffect(hitInfo);
        }

        var bullet = Instantiate(_bullet, _muzzle.position, _muzzle.rotation);
        bullet.Initialize(_muzzle.forward);
    }
}