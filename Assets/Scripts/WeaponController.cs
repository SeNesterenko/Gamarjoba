using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform[] _weaponSlots;
    [SerializeField] private Animator _rigLayer;
    
    private Weapon[] _equippedWeapons = new Weapon[2];
    private int _activeWeaponIndex;
    
    private bool _isWeaponEquip;
    private bool _isWeaponHolstered;
    
    private static readonly int IsHolstered = Animator.StringToHash("IsHolstered");

    private void Start()
    {
        _activeWeaponIndex = -1;
        var existingWeapon = GetComponentInChildren<Weapon>();

        if (existingWeapon)
        {
            EquipWeapon(existingWeapon);
        }
    }

    private void Update()
    {
        var weapon = GetWeapon(_activeWeaponIndex);
        _isWeaponEquip = weapon != null;

        if (_isWeaponEquip)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                ToggleActiveWeapon();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetActiveWeapon((int) WeaponSlots.Primary);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetActiveWeapon((int) WeaponSlots.Secondary);
            }

            if (_isWeaponHolstered)
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                weapon.StartShooting();
            }
            
            if (weapon.IsShooting)
            {
                weapon.UpdateShooting(Time.deltaTime);
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                weapon.StopShooting();   
            }
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        var weaponSlotIndex = newWeapon.GetWeaponSlot();
        var weapon = GetWeapon(weaponSlotIndex);
        
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }
        
        weapon = newWeapon;
        weapon.transform.SetParent(_weaponSlots[weaponSlotIndex], false);

        _equippedWeapons[weaponSlotIndex] = weapon;
        SetActiveWeapon(newWeapon.GetWeaponSlot());
    }

    private Weapon GetWeapon(int index)
    {
        return _equippedWeapons[index];
    }

    private void ToggleActiveWeapon()
    {
        var isHolstered = _rigLayer.GetBool(IsHolstered);

        if (isHolstered)
        {
            StartCoroutine(ActivateWeapon(_activeWeaponIndex));
        }
        else
        {
            StartCoroutine(HolsterWeapon(_activeWeaponIndex));
        }
    }
    
    private void SetActiveWeapon(int weaponSlot)
    {
        if (weaponSlot == _activeWeaponIndex)
        {
            return;
        }
        
        StartCoroutine(SwitchWeapon(_activeWeaponIndex,weaponSlot));
    }
    
    private IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    {
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        
        yield return new WaitForSeconds(0.5f); // TODO: remove this crutch
        
        yield return StartCoroutine(ActivateWeapon(activateIndex));
        _activeWeaponIndex = activateIndex;
    }
    
    private IEnumerator HolsterWeapon(int index)
    {
        var weapon = GetWeapon(index);
        _isWeaponHolstered = true;
        
        if (weapon)
        {
            _rigLayer.SetBool(IsHolstered, true);

            do
            {
                yield return new WaitForEndOfFrame();
            } while (_rigLayer.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }
    
    private IEnumerator ActivateWeapon(int index)
    {
        _isWeaponHolstered = false;
        var weapon = GetWeapon(index);

        if (weapon)
        {
            _rigLayer.SetBool(IsHolstered, false);
            _rigLayer.Play("Equip" + weapon.GetWeaponName());
            
            do
            {
                yield return new WaitForEndOfFrame();
            } while (_rigLayer.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }
}