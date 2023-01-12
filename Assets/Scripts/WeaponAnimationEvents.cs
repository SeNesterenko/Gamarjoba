using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : UnityEvent<string>
{
    
}
public class WeaponAnimationEvents : MonoBehaviour
{
    public AnimationEvent WeaponAnimationEvent { get; } = new ();

    private void OnAnimationEvent(string eventName)
    {
        WeaponAnimationEvent.Invoke(eventName);
    }
}