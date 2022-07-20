using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public bool battleState;

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

    [SerializeField] private float _battleStateTimeout = 3;
    [SerializeField] private float _battleStateTimeoutDelta;

    [SerializeField] private float attackSpeed;

    private Animator _animator;

    private bool _blockMovement;
    private float _gravity = -9.81f;
    private float _jumpTimeoutDelta;

    private CharacterController _characterController;


    void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();

        _animator.SetLayerWeight(1, 1);
        _mainCamera = Camera.main;

        InputService.OnJumpButtonDown += TryToJump;
    }

    void Update()
    {
        if (InputService.get == null)
        {
            Debug.LogError("Input service nulled");
            return;
        }

        GroundCheck();
        Movement();
        Rotation();
        Gravity();
        UpdateMotionAnimator();
        BattleStateTimer();

    }

    void BattleStateTimer()
    {
        if (battleState == false) return;

        if (_battleStateTimeoutDelta > 0)
        {
            _battleStateTimeoutDelta -= Time.deltaTime;

            if (_battleStateTimeoutDelta <= 0) battleState = false;
        }
    }

    void Rotation()
    {
        if (InputService.get == null)
        {
            Debug.LogError("Input service NULL");
            return;
        }

        Vector3 inputDirection = new Vector3(InputService.get.MoveInput.x, 0.0f, InputService.get.MoveInput.y).normalized;

        // Поворачиваем игрока по камере
        if (InputService.get.MoveInput != Vector2.zero || battleState)
        {
            // Если игрок в батл стейте то поворот по Z будет игнорироваться
            _targetRotation = Mathf.Atan2(battleState ? 0 : inputDirection.x, battleState ? 0 : inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;

            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _rotationSmoothTime);

            // Поворачиваем перса под угол камеры
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
    }

    void Movement()
    {
        if (InputService.get == null)
        {
            Debug.LogError("Input service NULL");
            return;
        }

        _moveDirection.x = InputService.get.MoveInput.x;
        _moveDirection.z = InputService.get.MoveInput.y;

        // Сначала проверяем бежит ли игрок вперед или назад
        bool backwardMove = InputService.get.MoveInput.y > 0 ? false : true;

        float targetMoveSpeed = 0;


        if (backwardMove)
            targetMoveSpeed = _moveSpeed;
        else
            targetMoveSpeed = _backwardMoveSpeed;



        if (InputService.get.MoveInput == Vector2.zero || _blockMovement) targetMoveSpeed = 0;

        if (_blockMovement) return;

        // Берем текущую скорость движения, без учета гравитации
        float currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z).magnitude;

        float speedOffset = 0.1f;

        // Для стиков на геймпаде
        float inputMagnitude = InputService.get.MoveInput.magnitude;

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
        Vector3 targetDir = new Vector3(InputService.get.MoveInput.x, 0, InputService.get.MoveInput.y);
        targetDir = _mainCamera.transform.TransformDirection(targetDir);
        targetDir = Vector3.ProjectOnPlane(targetDir, Vector3.up);

        Vector3 targetMovement = targetDir.normalized * _currentMoveSpeed * Time.deltaTime;

        // К нашему движению добавляем вертикальное ускорение, вертикальное ускорение меняется в зависимости от прыжков,падений и тд
        targetMovement += new Vector3(0, _verticalVelocity, 0) * Time.deltaTime;

        _characterController.Move(targetMovement);

        // Обновляем переменную для бленд движения аниматора 
        if (targetMoveSpeed > 0)
        {
            if (InputService.get.MoveInput.magnitude != 0)
            {
                // в бэтл стейте есть отыгрываем анимации во всех направлениях
                if (battleState)
                {
                    int verticalDir = 0;
                    if (InputService.get.MoveInput.y != 0)
                        verticalDir = InputService.get.MoveInput.y > 0 ? 1 : -1;

                    _verticalAnimationMotion = Mathf.Lerp(_verticalAnimationMotion
                            , 1 * verticalDir
                            , Time.deltaTime * _speedChangeRate);

                    int horizontalDir = 0;
                    if (InputService.get.MoveInput.x != 0)
                        horizontalDir = InputService.get.MoveInput.x > 0 ? 1 : -1;

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

    void TryToJump()
    {
        if (_jumpTimeoutDelta > 0 || !_isGrounded) return;

        Jump();
    }

    void Jump()
    {
        _animator.SetBool("Jump", true);
        _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    }

    void Gravity()
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
        }

        _verticalVelocity += _gravity * Time.deltaTime;
    }

    void GroundCheck()
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


    void OnLanded()
    {
        //TODO например всякие партиклы
    }

    void UpdateMotionAnimator()
    {
        _animator.SetFloat("Motion_Y", _verticalAnimationMotion);
        _animator.SetFloat("Motion_X", _horizontalAnimationMotion);
    }

    private void OnDestroy()
    {
        InputService.OnJumpButtonDown -= TryToJump;
    }
}
