using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private float _aimDuration;
    [SerializeField] private Rig _aimModeRigLayer;
    
    [SerializeField] private Weapon _weapon;

    private bool _isReadyShoot;
    
    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _aimModeRigLayer.weight += Time.deltaTime / _aimDuration;
            _isReadyShoot = true;
        }
        else
        {
            _aimModeRigLayer.weight -= Time.deltaTime / _aimDuration;
            _isReadyShoot = false;
        }

        if (_isReadyShoot)
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
}