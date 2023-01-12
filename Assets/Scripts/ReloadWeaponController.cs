using System;
using UnityEngine;

// TODO: remove magazine duplication
public class ReloadWeaponController : MonoBehaviour
{
    private WeaponAnimationEvents _weaponAnimationEvents;
    private Animator _rigController;

    private Weapon _weapon;
    private Transform _leftHand;
    private GameObject _magazineHand;
    
    private static readonly int IsReloading = Animator.StringToHash("IsReloading");

    private void Start()
    {
        _weaponAnimationEvents.WeaponAnimationEvent.AddListener(OnAnimationEvent);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || _weapon.AmmoCount <= 0)
        {
            _rigController.SetTrigger(IsReloading);
        }
    }

    public void Initialize(Animator rigController, Weapon weapon, Transform leftHand)
    {
        _rigController = rigController;
        _weaponAnimationEvents = rigController.GetComponent<WeaponAnimationEvents>();
        _weapon = weapon;
        _leftHand = leftHand;
    }

    private void OnAnimationEvent(string eventName)
    {
        if (Enum.TryParse(eventName, out StepsReloadingWeapon stepsReloadingWeapon))
        {
            switch (stepsReloadingWeapon)
            {
                case StepsReloadingWeapon.DetachMagazine :
                    DetachMagazine();
                    break;
                
                case StepsReloadingWeapon.DropMagazine :
                    DropMagazine();
                    break;
                
                case StepsReloadingWeapon.RefillMagazine :
                    RefillMagazine();
                    break;
                
                case StepsReloadingWeapon.AttachMagazine :
                    AttachMagazine();
                    break;
            }
        }
    }

    private void DetachMagazine()
    {
        _magazineHand = Instantiate(_weapon.Magazine, _leftHand, true);
        _weapon.Magazine.SetActive(false);
    }
    
    private void DropMagazine()
    {
        var droppedMagazine =
            Instantiate(_magazineHand, _magazineHand.transform.position, _magazineHand.transform.rotation);

        droppedMagazine.AddComponent<Rigidbody>();
        droppedMagazine.AddComponent<BoxCollider>();

        _magazineHand.SetActive(false);
    }
    
    private void RefillMagazine()
    {
        _magazineHand.SetActive(true);
    }
    
    private void AttachMagazine()
    {
        _weapon.Magazine.SetActive(true);
        Destroy(_magazineHand);
        _weapon.SetAmmoCount(_weapon.MagazineSize);
        _rigController.ResetTrigger(IsReloading);
    }
}