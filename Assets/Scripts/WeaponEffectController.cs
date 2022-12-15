using UnityEngine;

public class WeaponEffectController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _shootEffect;
    [SerializeField] private ParticleSystem _hitEffect;
    
    public void PlayShootEffect()
    {
        _shootEffect.Emit(1);
    }

    public void PlayHitEffect(RaycastHit hitInfo)
    {
        _hitEffect.transform.position = hitInfo.point;
        _hitEffect.transform.forward = hitInfo.normal;
        
        _hitEffect.Emit(1);
    }
}