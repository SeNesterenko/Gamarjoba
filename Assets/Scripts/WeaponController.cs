using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform _weaponPivot;
    [SerializeField] private Animator _rigLayer;
    
    private Weapon _weapon;
    private bool _isWeaponEquip;
    private static readonly int IsHolstered = Animator.StringToHash("IsHolstered");

    private void Start()
    {
        var existingWeapon = GetComponentInChildren<Weapon>();

        if (existingWeapon)
        {
            EquipWeapon(existingWeapon);
        }
    }

    private void Update()
    {
        _isWeaponEquip = _weapon != null;

        if (_isWeaponEquip)
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

            if (Input.GetKeyDown(KeyCode.X))
            {
                var isHolstered = _rigLayer.GetBool(IsHolstered);
                _rigLayer.SetBool(IsHolstered, !isHolstered);
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
        
        _rigLayer.Play("Equip" + _weapon.GetWeaponName());
    }
}