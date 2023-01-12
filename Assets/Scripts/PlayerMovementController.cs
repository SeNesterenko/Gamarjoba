using UnityEngine;

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

    private void Update()
    {
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");
        
        _animator.SetFloat(InputX, _input.x);
        _animator.SetFloat(InputY, _input.y);

        UpdateIsSprinting();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (_isJumping)
        {
            _velocity.y -= _gravity * Time.fixedDeltaTime;
            _characterController.Move(_velocity * Time.fixedDeltaTime);

            _isJumping = !_characterController.isGrounded;
            _rootMotion = Vector3.zero;
            return;
        }
        
        _characterController.Move(_rootMotion + Vector3.down * _stepDown);
        _rootMotion = Vector3.zero;

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

    private void OnAnimatorMove()
    {
        _rootMotion += _animator.deltaPosition;
    }

    private void Jump()
    {
        if (!_isJumping)
        {
            _isJumping = true;
            _animator.SetBool(IsJumping, true);
            
            _velocity = _animator.velocity;
            _velocity.y = Mathf.Sqrt(2 * _gravity * _jumpHeight);
        }
    }

    private void UpdateIsSprinting()
    {
        var isSprinting = Input.GetKey(KeyCode.LeftShift);
        _animator.SetBool(IsSprinting, isSprinting);
    }
}