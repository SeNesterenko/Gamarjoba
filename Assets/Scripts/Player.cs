using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private AmmoView _ammoView;

    public void Start()
    {
        _weaponController.OnShot += DecreaseAmmoCount;
    }

    private void DecreaseAmmoCount(int ammoCount)
    {
        _ammoView.RefreshAmmo(ammoCount);
    }

    public void OnDestroy()
    {
        _weaponController.OnShot -= DecreaseAmmoCount;
    }
}