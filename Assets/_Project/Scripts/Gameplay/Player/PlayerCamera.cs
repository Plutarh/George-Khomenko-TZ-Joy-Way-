using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [SerializeField] private float _topClamp = 70.0f;
    [SerializeField] private float _bottomClamp = -30.0f;

    [SerializeField] private float _cameraAngleOverride = 0.0f;

    [SerializeField] private bool _lockCameraPosition = false;

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private Transform _followTarget;

    [SerializeField] private LayerMask cameraCollidersLayer;
    [SerializeField] private float _rotationMultiplier = 5;

    private InputService _inputService;
    private Cinemachine3rdPersonFollow _thirdPersonFollow;


    private const float THRESHOLD = 0.01f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;


    private Player _player;


    private void LateUpdate()
    {
        if (_player == null) return;
        if (Cursor.lockState != CursorLockMode.Locked) return;
        CameraRotation();
    }

    public void Initialize(Player player, InputService inputService)
    {
        GetComponents();

        _player = player;
        _inputService = inputService;
        _followTarget = _player.CameraRoot;
        _virtualCamera.Follow = _followTarget;

        _thirdPersonFollow.ShoulderOffset = new Vector3(-0.5f, 0.5f, 0);
        _thirdPersonFollow.CameraDistance = 4.3f;


        _thirdPersonFollow.CameraCollisionFilter = cameraCollidersLayer;

    }

    void GetComponents()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _thirdPersonFollow = _virtualCamera.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    private void CameraRotation()
    {
        if (_inputService.LookInput.sqrMagnitude >= THRESHOLD && !_lockCameraPosition)
        {
            float deltaTimeMultiplier = 1;

            _cinemachineTargetYaw += _inputService.LookInput.x * deltaTimeMultiplier * _rotationMultiplier;
            _cinemachineTargetPitch += -_inputService.LookInput.y * deltaTimeMultiplier * _rotationMultiplier;

        }

        // Огранчиваем повороты до 360 градусов
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);

        _player.CameraRoot.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + _cameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void OnDestroy()
    {

    }
}
