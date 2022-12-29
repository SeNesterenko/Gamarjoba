using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : UnityEvent<string>
{
    
}
public class WeaponAnimationEvents : MonoBehaviour
{
    public AnimationEvent WeaponAnimationEvent => _weaponAnimationEvent;
    
    private AnimationEvent _weaponAnimationEvent = new ();

    private void OnAnimationEvent(string eventName)
    {
        _weaponAnimationEvent.Invoke(eventName);
    }
}