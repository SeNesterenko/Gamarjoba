using UnityEngine;
using UnityEngine.Events;

public class PickableWeapon : MonoBehaviour
{
    [SerializeField] private Weapon _weaponPrefab;
    [SerializeField] private UnityEvent<Weapon> _weaponPickedUp;
    
    private void OnTriggerEnter(Collider other)
    {
        var newWeapon = Instantiate(_weaponPrefab);
        _weaponPickedUp.Invoke(newWeapon);
    }
}