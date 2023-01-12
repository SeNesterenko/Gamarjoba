using Cinemachine;
using UnityEngine;

public class WeaponEffectController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _shootEffect;
    [SerializeField] private ParticleSystem _hitEffect;
    
    [SerializeField] private float _verticalRecoil;
    [SerializeField] private float _recoilDuration;
    
    [SerializeField] private CinemachineImpulseSource _cameraShake;
    [SerializeField] private Animator _rigController;
    
    [SerializeField] private CinemachineFreeLook _playerCamera;
    
    private float _time;

    private void Update()
    {
        if (_time > 0)
        {
            _playerCamera.m_YAxis.Value -= _verticalRecoil * Time.deltaTime / _recoilDuration;
            _time -= Time.deltaTime;
        }
    }

    public void Initialize(CinemachineFreeLook playerCamera, Animator rigLayer)
    {
        _playerCamera = playerCamera;
        _rigController = rigLayer;
    }
    
    public void GenerateRecoil(string weaponName)
    {
        _time = _recoilDuration;
        _cameraShake.GenerateImpulse(Camera.main.transform.forward);
        
        _rigController.Play("WeaponRecoil" + weaponName, 1, 0f);
    }

    public void PlayShootEffect()
    {
        _shootEffect.Emit(1);
    }

    public void PlayHitEffect(RaycastHit hitInfo)
    {
        _hitEffect.transform.position = hitInfo.point;
        _hitEffect.transform.forward = hitInfo.normal;
        
        _hitEffect.Emit(1);
    }
}