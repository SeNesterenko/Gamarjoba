using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerRotationController))]
public class Player : MonoBehaviour
{
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private AmmoView _ammoView;
    
    private PlayerMovementController _playerMovementController;

    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
        _playerInputActions = new PlayerInputActions();
    }

    private void Start()
    {
        OnEnable();
        
        _playerInputActions.Player.Jump.performed += _playerMovementController.OnJump;

        _playerInputActions.Player.ToggleActiveWeapon.performed += _weaponController.ToggleActiveWeapon;
        _playerInputActions.Player.SetPrimaryWeapon.performed += _weaponController.SetPrimaryWeapon;
        _playerInputActions.Player.SetSecondaryWeapon.performed += _weaponController.SetSecondaryWeapon;

        _weaponController.OnShot += DecreaseAmmoCount;
        _weaponController.OnWeaponChanged += DecreaseAmmoCount;
    }

    private void Update()
    {
        var inputMoveValue = _playerInputActions.Player.Move.ReadValue<Vector2>();
        _playerMovementController.Move(inputMoveValue);

        var inputSprintValue = _playerInputActions.Player.Sprint.ReadValue<float>();
        _playerMovementController.Sprint(inputSprintValue);
        _weaponController.WeaponSprint(inputSprintValue);
        
        var inputShootValue = _playerInputActions.Player.Shoot.ReadValue<float>();
        _weaponController.ControlShooting(inputShootValue);
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Disable();
    }

    private void DecreaseAmmoCount(int ammoCount)
    {
        _ammoView.RefreshAmmo(ammoCount);
    }

    public void OnDestroy()
    {
        _playerInputActions.Player.Jump.performed -= _playerMovementController.OnJump;
        
        _playerInputActions.Player.ToggleActiveWeapon.performed -= _weaponController.ToggleActiveWeapon;
        _playerInputActions.Player.SetPrimaryWeapon.performed -= _weaponController.SetPrimaryWeapon;
        _playerInputActions.Player.SetSecondaryWeapon.performed -= _weaponController.SetSecondaryWeapon;

        _weaponController.OnShot -= DecreaseAmmoCount;
        _weaponController.OnWeaponChanged -= DecreaseAmmoCount;
    }
}