using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravity;
    [SerializeField] private float _stepDown;
    
    private Vector3 _velocity;
    private bool _isJumping;
    
    private Vector3 _rootMotion;
    private CharacterController _characterController;
    
    private Animator _animator;
    private Vector2 _input;
    
    private static readonly int InputX = Animator.StringToHash("InputX");
    private static readonly int InputY = Animator.StringToHash("InputY");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsSprinting = Animator.StringToHash("IsSprinting");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (_isJumping)
        {
            UseGravityOnCharacter();
            return;
        }
        
        _characterController.Move(_rootMotion + Vector3.down * _stepDown);
        _rootMotion = Vector3.zero;

        CheckCharacterOnGround();
    }

    private void OnAnimatorMove()
    {
        _rootMotion += _animator.deltaPosition;
    }

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        if (!_isJumping)
        {
            _isJumping = true;
            _animator.SetBool(IsJumping, true);
            
            _velocity = _animator.velocity;
            _velocity.y = Mathf.Sqrt(2 * _gravity * _jumpHeight);
        }
    }

    public void Sprint(float inputValue)
    {
        var isSprinting = inputValue > 0;
        _animator.SetBool(IsSprinting, isSprinting);
    }

    public void Move(Vector2 inputValue)
    {
        _animator.SetFloat(InputX, inputValue.x);
        _animator.SetFloat(InputY, inputValue.y);
    }
    
    private void CheckCharacterOnGround()
    {
        if (!_characterController.isGrounded)
        {
            _isJumping = true;
            _animator.SetBool(IsJumping, true);
            _velocity = _animator.velocity;
            _velocity.y = 0;
        }
        else
        {
            _animator.SetBool(IsJumping, false);
        }
    }
    
    private void UseGravityOnCharacter()
    {
        _velocity.y -= _gravity * Time.fixedDeltaTime;
        _characterController.Move(_velocity * Time.fixedDeltaTime);

        _isJumping = !_characterController.isGrounded;
        _rootMotion = Vector3.zero;
    }
}