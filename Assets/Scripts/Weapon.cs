using System;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(WeaponEffectController))]
[RequireComponent(typeof(ReloadWeaponController))]
public class Weapon : MonoBehaviour
{
    public event Action<int> OnShot;
    
    [SerializeField] private Transform _muzzle;
    [SerializeField] private float _rateShootPerSecond = 0.04f;

    [SerializeField] private string _weaponName;
    [SerializeField] private WeaponSlots _weaponSlot;
    
    [SerializeField] private Bullet _bullet;
    [SerializeField] private GameObject _magazine;

    [SerializeField] private int _ammoCount;
    [SerializeField] private int _magazineSize;

    public bool IsShooting { get; private set; }
    public int MagazineSize => _magazineSize;
    public int AmmoCount => _ammoCount;
    public GameObject Magazine => _magazine;
    
    private float _accumulatedTime;
    private float _timeForOneShot;

    private Ray _ray;
    private WeaponEffectController _weaponEffectController;
    private ReloadWeaponController _reloadWeaponController;

    private void Start()
    {
        _timeForOneShot = 1f / _rateShootPerSecond;
    }

    public void Initialize(CinemachineFreeLook playerCamera, Animator rigController, Transform leftHand)
    {
        _reloadWeaponController = GetComponent<ReloadWeaponController>();
        _reloadWeaponController.Initialize(rigController, gameObject.GetComponent<Weapon>(), leftHand);
        
        _weaponEffectController = GetComponent<WeaponEffectController>();
        _weaponEffectController.Initialize(playerCamera, rigController);
    }

    public void SetAmmoCount(int ammoCount)
    {
        _ammoCount = ammoCount;
        OnShot?.Invoke(_ammoCount);
    }
    
    public string GetWeaponName()
    {
        return _weaponName;
    }

    public int GetWeaponSlot()
    {
        return (int) _weaponSlot;
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
        if (_ammoCount <= 0)
        {
            return;
        }
        
        _ammoCount--;
        OnShot?.Invoke(_ammoCount);
        
        _weaponEffectController.PlayShootEffect();
        _weaponEffectController.GenerateRecoil(_weaponName);
        
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