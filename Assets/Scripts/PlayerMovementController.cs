using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Animator _animator;
    private Vector2 _input;
    
    private static readonly int InputX = Animator.StringToHash("InputX");
    private static readonly int InputY = Animator.StringToHash("InputY");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");
        
        _animator.SetFloat(InputX, _input.x);
        _animator.SetFloat(InputY, _input.y);
    }
}