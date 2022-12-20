using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private float _aimDuration;
    [SerializeField] private Rig _aimModeRigLayer;

    [SerializeField] private Transform _weaponPivot;
    [SerializeField] private Rig _rigLayerHand;

    [SerializeField] private Transform _weaponRightHandGrip;
    [SerializeField] private Transform _weaponLeftHandGrip;

    private Weapon _weapon;

    private Animator _animator;
    private AnimatorOverrideController _animatorOverride;

    private bool _isWeaponEquip;
    private bool _isReadyShoot;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animatorOverride = _animator.runtimeAnimatorController as AnimatorOverrideController;
        
        var existingWeapon = GetComponentInChildren<Weapon>();

        if (existingWeapon)
        {
            EquipWeapon(existingWeapon);
        }
    }

    private void Update()
    {
        _isReadyShoot = _aimModeRigLayer.weight >= 1;
        _isWeaponEquip = _weapon != null;
        
        if (!_isWeaponEquip)
        {
            _rigLayerHand.weight = 0f;
            _animator.SetLayerWeight(1, 0f);
            return;
        }

        _rigLayerHand.weight = 1f;

        if (_isReadyShoot && _isWeaponEquip)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _weapon.StartShooting();
            }
            
            if (_weapon.IsShooting)
            {
                _weapon.UpdateShooting(Time.deltaTime);
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                _weapon.StopShooting();   
            }
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        if (_weapon)
        {
            Destroy(_weapon.gameObject);
        }
        
        _weapon = newWeapon;
        _weapon.transform.parent = _weaponPivot;
        
        _weapon.transform.localPosition = Vector3.zero;
        _weapon.transform.localRotation = Quaternion.identity;
        
        _animator.SetLayerWeight(1, 1f);
        _animatorOverride["WeaponAnimationEmpty"] = _weapon.GetWeaponAnimation();
    }

    [ContextMenu("Save weapon pose")]
    private void SaveWeaponPose()
    {
        var recorder = new GameObjectRecorder(gameObject);
        
        recorder.BindComponentsOfType<Transform>(_weaponPivot.gameObject, false);
        recorder.BindComponentsOfType<Transform>(_weaponLeftHandGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(_weaponRightHandGrip.gameObject, false);
        
        recorder.TakeSnapshot(0f);

        var weaponAnimation = _weapon.GetWeaponAnimation();
        recorder.SaveToClip(weaponAnimation);
        
        UnityEditor.AssetDatabase.SaveAssets();
    }
}