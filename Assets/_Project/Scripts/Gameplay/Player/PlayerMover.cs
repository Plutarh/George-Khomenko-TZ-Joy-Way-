using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _targetRotation;

    [Header("Animations Motion")]
    [SerializeField] private float _horizontalAnimationMotion;
    [SerializeField] private float _verticalAnimationMotion;

    [Space]
    [Range(0.0f, 0.3f)]
    [SerializeField] private float _rotationSmoothTime = 0.12f;
    [SerializeField] private float _rotationVelocity;
    [SerializeField] private Vector3 _targetDirection;
    [SerializeField] private Camera _mainCamera;

    [Header("Movement")]
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _backwardMoveSpeed;
    [SerializeField] protected float _sprintSpeed;
    [SerializeField] protected float _speedChangeRate;
    private Vector3 _moveDirection;

    [Space]
    [SerializeField] protected LayerMask _groundLayers;
    [SerializeField] protected bool _isGrounded;
    [SerializeField] protected Transform _groundChecker;

    [Space]
    [SerializeField] private float _currentMoveSpeed;


    [SerializeField] private float _fallTimeoutDelta;
    [SerializeField] private float _fallTimeout = 0.2f;

    [Space]
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpTimeout = 0.1f;
    [SerializeField] private float _verticalVelocity;



    [SerializeField] private float attackSpeed;
    [SerializeField] private LayerMask aimLayers;

    private Animator _animator;
    private bool _battleState;
    private bool _blockMovement;
    private float _gravity = -9.81f;
    private float _jumpTimeoutDelta;
    private InputService _inputService;


    private CharacterController _characterController;


    public void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _animator.SetLayerWeight(1, 1);
        _mainCamera = Camera.main;
    }

    public void Start()
    {
        _inputService = InputService.get;
    }

    public void Update()
    {
        if (_inputService == null)
        {
            Debug.LogError("Input service nulled");
            return;
        }

        GroundCheck();
        Movement();
        Rotation();
        Gravity();
        UpdateMotionAnimator();

        if (Input.GetKeyDown(KeyCode.Space))
            TryToJump();
    }


    public Vector3 GetAimDirection()
    {
        return _mainCamera.transform.forward;
    }

    public Vector3 GetAimPoint()
    {
        Vector3 rayPoint = Vector3.zero;

        RaycastHit hit;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        float raycastDistance = 1600;

        if (Physics.Raycast(ray, out hit, raycastDistance, aimLayers))
            rayPoint = hit.point;
        else
            rayPoint = ray.GetPoint(raycastDistance);

        return rayPoint;
    }

    public void Rotation()
    {
        if (_inputService == null)
        {
            Debug.LogError("Input service NULL");
            return;
        }


        Vector3 inputDirection = new Vector3(_inputService.MoveInput.x, 0.0f, _inputService.MoveInput.y).normalized;

        // Поворачиваем игрока по камере
        if (_inputService.MoveInput != Vector2.zero || _battleState)
        {
            // Если игрок в батл стейте то поворот по Z будет игнорироваться
            _targetRotation = Mathf.Atan2(_battleState ? 0 : inputDirection.x, _battleState ? 0 : inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;

            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _rotationSmoothTime);

            // Поворачиваем перса под угол камеры
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
    }

    public void Movement()
    {
        if (_inputService == null)
        {
            Debug.LogError("Input service NULL");
            return;
        }

        _moveDirection.x = _inputService.MoveInput.x;
        _moveDirection.z = _inputService.MoveInput.y;

        // Сначала проверяем бежит ли игрок вперед или назад
        bool backwardMove = _inputService.MoveInput.y > 0 ? false : true;

        float targetMoveSpeed = 0;


        if (backwardMove)
            targetMoveSpeed = _moveSpeed;
        else
            targetMoveSpeed = _backwardMoveSpeed;



        if (_inputService.MoveInput == Vector2.zero || _blockMovement) targetMoveSpeed = 0;

        if (_blockMovement) return;

        // Берем текущую скорость движения, без учета гравитации
        float currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z).magnitude;

        float speedOffset = 0.1f;

        // Для стиков на геймпаде
        float inputMagnitude = _inputService.MoveInput.magnitude;

        if (currentHorizontalSpeed < targetMoveSpeed - speedOffset || currentHorizontalSpeed > targetMoveSpeed + speedOffset)
        {
            _currentMoveSpeed = Mathf.Lerp(currentHorizontalSpeed, targetMoveSpeed * inputMagnitude, _speedChangeRate * Time.deltaTime);

            // Округляем, до более нормальных чисел
            _currentMoveSpeed = Mathf.Round(_currentMoveSpeed * 1000f) / 1000f;
        }
        else
        {
            _currentMoveSpeed = targetMoveSpeed;
        }

        // Игрок будет двигаться по форварду камеры
        Vector3 targetDir = new Vector3(_inputService.MoveInput.x, 0, _inputService.MoveInput.y);
        targetDir = _mainCamera.transform.TransformDirection(targetDir);
        targetDir = Vector3.ProjectOnPlane(targetDir, Vector3.up);

        Vector3 targetMovement = targetDir.normalized * _currentMoveSpeed * Time.deltaTime;

        // К нашему движению добавляем вертикальное ускорение, вертикальное ускорение меняется в зависимости от прыжков,падений и тд
        targetMovement += new Vector3(0, _verticalVelocity, 0) * Time.deltaTime;

        _characterController.Move(targetMovement);

        // Обновляем переменную для бленд движения аниматора 
        if (targetMoveSpeed > 0)
        {
            if (_inputService.MoveInput.magnitude != 0)
            {
                // в бэтл стейте есть отыгрываем анимации во всех направлениях
                if (_battleState)
                {
                    int verticalDir = 0;
                    if (_inputService.MoveInput.y != 0)
                        verticalDir = _inputService.MoveInput.y > 0 ? 1 : -1;

                    _verticalAnimationMotion = Mathf.Lerp(_verticalAnimationMotion
                            , 1 * verticalDir
                            , Time.deltaTime * _speedChangeRate);

                    int horizontalDir = 0;
                    if (_inputService.MoveInput.x != 0)
                        horizontalDir = _inputService.MoveInput.x > 0 ? 1 : -1;

                    _horizontalAnimationMotion = Mathf.Lerp(_horizontalAnimationMotion
                            , 1 * horizontalDir
                            , Time.deltaTime * _speedChangeRate);

                }
                // Вне бэтл стейта, всегда играется анимация бега вперед, горизонтальных анимаций нету
                else
                {
                    _verticalAnimationMotion = Mathf.Lerp(_verticalAnimationMotion, 1, _currentMoveSpeed / _moveSpeed);
                    _horizontalAnimationMotion = 0;
                }
            }
            else
            {
                _verticalAnimationMotion = 0;
                _horizontalAnimationMotion = 0;
            }
        }
        else
        {
            // Если таргет скорость равна нулю, то просто плавно сбавляем бленд
            _verticalAnimationMotion = Mathf.Lerp(_verticalAnimationMotion, 0, Time.deltaTime * _speedChangeRate);
            _horizontalAnimationMotion = 0;
        }

        _verticalAnimationMotion = Mathf.Clamp(_verticalAnimationMotion, -1, 1);
        _horizontalAnimationMotion = Mathf.Clamp(_horizontalAnimationMotion, -1, 1);
    }

    public void TryToJump()
    {
        if (_jumpTimeoutDelta > 0 || !_isGrounded) return;

        Jump();
    }

    public void Jump()
    {
        _animator.SetBool("Jump", true);
        _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    }

    public void Gravity()
    {
        if (_isGrounded)
        {
            _animator.SetBool("FreeFall", false);
            _animator.SetBool("Jump", false);

            _fallTimeoutDelta = _fallTimeout;

            // не уходим в бесконечность
            if (_verticalVelocity < 0)
                _verticalVelocity = -2f;

            // Откатываем кд прыжка
            if (_jumpTimeoutDelta >= 0.0f)
                _jumpTimeoutDelta -= Time.deltaTime;
        }
        else
        {
            _jumpTimeoutDelta = _jumpTimeout;

            if (_fallTimeoutDelta > 0)
                _fallTimeoutDelta -= Time.deltaTime;
            else
                _animator.SetBool("FreeFall", true);

            // if (_inputService != null)
            //     _inputService.ResetJump();
            // else
            //     Debug.LogError("Player Input service NULL");
        }

        _verticalVelocity += _gravity * Time.deltaTime;
    }

    public void GroundCheck()
    {
        Vector3 spherePosition = Vector3.zero;

        // Если есть отдельный трансформ для граунд чека, то юзаем его, если нету, то юзаем обычный трансформ 
        if (_groundChecker != null)
            spherePosition = new Vector3(_groundChecker.position.x, _groundChecker.position.y - 0.15f, _groundChecker.position.z);
        else
            spherePosition = new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z);

        bool lastCheck = _isGrounded;
        _isGrounded = Physics.CheckSphere(spherePosition, 0.3f, _groundLayers, QueryTriggerInteraction.Ignore);

        if (lastCheck == false && _isGrounded) OnLanded();

        _animator.SetBool("Land", _isGrounded);
    }


    public void OnLanded()
    {

    }

    public void UpdateMotionAnimator()
    {
        _animator.SetFloat("Motion_Y", _verticalAnimationMotion);
        _animator.SetFloat("Motion_X", _horizontalAnimationMotion);
    }
}