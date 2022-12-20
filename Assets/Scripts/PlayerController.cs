using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerRotationController))]
[RequireComponent(typeof(WeaponController))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovementController _playerMovementController;
    private PlayerRotationController _playerRotationController;
    private WeaponController _weaponController;

    private void Start()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
        _playerRotationController = GetComponent<PlayerRotationController>();
        _weaponController = GetComponent<WeaponController>();
    }

    private void Update()
    {
        
    }
}