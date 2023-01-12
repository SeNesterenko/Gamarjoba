using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerRotationController))]
public class Player : MonoBehaviour
{
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private AmmoView _ammoView;
    
    private PlayerMovementController _playerMovementController;
    private PlayerRotationController _playerRotationController;
    
    public void Start()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
        _playerRotationController = GetComponent<PlayerRotationController>();
        
        _weaponController.OnShot += DecreaseAmmoCount;
        _weaponController.OnWeaponChanged += DecreaseAmmoCount;
    }

    private void DecreaseAmmoCount(int ammoCount)
    {
        _ammoView.RefreshAmmo(ammoCount);
    }

    public void OnDestroy()
    {
        _weaponController.OnShot -= DecreaseAmmoCount;
        _weaponController.OnWeaponChanged += DecreaseAmmoCount;
    }
}