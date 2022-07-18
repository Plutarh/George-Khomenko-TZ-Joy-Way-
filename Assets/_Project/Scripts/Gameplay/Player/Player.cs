using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(PlayerMover))]
public class Player : Pawn
{
    public Transform CameraRoot => _cameraRoot;

    [SerializeField] private Transform _cameraRoot;
    private InputService _inputService;
    [SerializeField] private LayerMask aimLayers;

    Camera _mainCamera;

    [SerializeField] private Weapon _leftHandWeapon;
    [SerializeField] private Weapon _rightHandWeapon;

    [SerializeField] private Transform _leftWeaponIKParent;
    [SerializeField] private Transform _rightWeaponIKParent;

    [SerializeField] private TwoBoneIKConstraint _leftHandIK;
    [SerializeField] private TwoBoneIKConstraint _rightHandIK;

    public override void Awake()
    {
        base.Awake();

        _mainCamera = Camera.main;

        ResetIK(_leftHandIK);
        ResetIK(_rightHandIK);
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {
            _leftHandWeapon.Shoot(GetAimPoint());
        }
        if (Input.GetMouseButtonDown(1))
        {
            _rightHandWeapon.Shoot(GetAimPoint());
        }
    }

    void ResetIK(TwoBoneIKConstraint iKConstraint)
    {
        iKConstraint.weight = 0;
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
}
