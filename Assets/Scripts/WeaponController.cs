using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public event Action<int> OnShot;
    public event Action<int> OnWeaponChanged;
    
    [SerializeField] private CinemachineFreeLook _playerCamera;
    [SerializeField] private Animator _rigController;
    [SerializeField] private Transform _leftHand;
    
    [SerializeField] private Transform[] _weaponSlots;

    private readonly Weapon[] _equippedWeapons = new Weapon[2];
    private int _activeWeaponIndex;
    
    private bool _isWeaponEquip;
    private bool _isWeaponHolstered;

    private static readonly int IsHolstered = Animator.StringToHash("IsHolstered");
    private static readonly int WeaponIndex = Animator.StringToHash("WeaponIndex");

    private void Start()
    {
        _activeWeaponIndex = -1;
        
        var existingWeapon = GetComponentInChildren<Weapon>();

        if (existingWeapon)
        {
            EquipWeapon(existingWeapon);
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        var weaponSlotIndex = newWeapon.GetWeaponSlot();
        var weapon = GetWeapon(weaponSlotIndex);
        
        if (weapon)
        {
            weapon.OnShot -= DecreaseAmmoCount;
            Destroy(weapon.gameObject);
        }

        weapon = newWeapon;
        weapon.OnShot += DecreaseAmmoCount;
        weapon.Initialize(_playerCamera, _rigController, _leftHand);
        weapon.transform.SetParent(_weaponSlots[weaponSlotIndex], false);

        _equippedWeapons[weaponSlotIndex] = weapon;

        SetActiveWeapon(weaponSlotIndex);
        _isWeaponEquip = true;
    }

    public void ToggleActiveWeapon(InputAction.CallbackContext callbackContext)
    {
        if (_isWeaponEquip)
        {
            var isHolstered = _rigController.GetBool(IsHolstered);
            StartCoroutine(isHolstered ? ActivateWeapon(_activeWeaponIndex) : HolsterWeapon(_activeWeaponIndex));
        }
    }

    public void SetPrimaryWeapon(InputAction.CallbackContext callbackContext)
    {
        if (_isWeaponEquip)
        {
           SetActiveWeapon((int) WeaponSlots.Primary); 
        }
    }
    
    public void SetSecondaryWeapon(InputAction.CallbackContext callbackContext)
    {
        if (_isWeaponEquip)
        {
            SetActiveWeapon((int) WeaponSlots.Secondary);
        }
    }

    public void ControlShooting(float inputValue)
    {
        if (_isWeaponEquip)
        {
            if (_isWeaponHolstered)
            {
                return;
            }

            var weapon = GetWeapon(_activeWeaponIndex);
                    
            if (inputValue > 0 && !weapon.IsShooting)
            {
                weapon.StartShooting();
            }

            if (weapon.IsShooting)
            {
                weapon.UpdateShooting(Time.deltaTime);
            }
            
            if (inputValue == 0)
            {
                weapon.StopShooting();
            }
        }
    }

    private Weapon GetWeapon(int index)
    {
        return index != -1 ?_equippedWeapons[index] : null;
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
        _rigController.SetInteger(WeaponIndex, activateIndex);
        
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
            _rigController.SetBool(IsHolstered, true);

            do
            {
                yield return new WaitForEndOfFrame();
            } while (_rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }
    
    private IEnumerator ActivateWeapon(int index)
    {
        _isWeaponHolstered = false;
        var weapon = GetWeapon(index);

        if (weapon)
        {
            _rigController.SetBool(IsHolstered, false);
            _rigController.Play("Equip" + weapon.GetWeaponName());
            
            do
            {
                yield return new WaitForEndOfFrame();
            } while (_rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }

        OnWeaponChanged?.Invoke(weapon.AmmoCount);
    }
    
    private void DecreaseAmmoCount(int ammoCount)
    {
        OnShot?.Invoke(ammoCount);
    }
}