using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private List<PlayerWeapons> _playerWeapons = new List<PlayerWeapons>();

    private PlayerMover _playerMover;
    private ObjectPicker _objectPicker;

    public override void Awake()
    {
        base.Awake();

        _mainCamera = Camera.main;
        _playerMover = GetComponent<PlayerMover>();
        _objectPicker = GetComponent<ObjectPicker>();

        InputService.OnAttackButtonDown += TryToAttack;
        InputService.OnHandPickDropButtonDown += DropWeaponFromHand;

        _objectPicker.OnPickedUpObject += TryEquipWeapon;

        ResetHandsIK();
    }

    public override void Update()
    {
        base.Update();
    }

    void TryToAttack(EHandType handType)
    {
        var foundedHand = _playerWeapons.FirstOrDefault(pw => pw.handType == handType);

        if (foundedHand == null)
        {
            Debug.LogError($"Cannot find hand : {handType}");
            return;
        }

        var weaponInHand = foundedHand.weapon;

        if (weaponInHand == null)
        {
            Debug.LogError($"Cannot find weapon in hand : {handType}");
            return;
        }

        _playerMover.battleState = true;
        weaponInHand.Shoot(GetAimPoint());
    }

    void DropWeaponFromHand(EHandType handType)
    {
        var foundedHand = _playerWeapons.FirstOrDefault(pw => pw.handType == handType);

        if (foundedHand == null)
        {
            Debug.LogError($"Cannot find hand : {handType}");
            return;
        }

        var weaponInHand = foundedHand.weapon;

        if (weaponInHand == null)
        {
            Debug.LogError($"Cannot find weapon in hand : {handType}");
            return;
        }

        weaponInHand.transform.SetParent(null);
        weaponInHand.Drop();
        weaponInHand = null;
    }

    void TryEquipWeapon(IPickable pickable, EHandType handType)
    {
        if (!(pickable is Weapon)) return;
        EquipWeapon(pickable as Weapon, handType);
    }

    void EquipWeapon(Weapon weapon, EHandType handType)
    {
        var foundedHand = _playerWeapons.FirstOrDefault(pw => pw.handType == handType);

        if (foundedHand == null) return;

        if (foundedHand.weapon != null) foundedHand.weapon.Drop();

        foundedHand.weapon = weapon;

        if (weapon.IsIK)
        {
            SmoothEnableHandIK(foundedHand.handIK);
            weapon.transform.SetParent(foundedHand.weaponIKParent);
        }
        else
        {
            weapon.transform.SetParent(foundedHand.weaponParent);
        }

        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        Debug.LogError("Setup new pos");
    }

    void SmoothEnableHandIK(TwoBoneIKConstraint iKConstraint)
    {
        iKConstraint.weight = 1;
    }

    void ResetHandsIK()
    {
        foreach (var playerWeapon in _playerWeapons)
        {
            playerWeapon.handIK.weight = 0;
        }
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

    private void OnDestroy()
    {
        InputService.OnAttackButtonDown -= TryToAttack;
        InputService.OnHandPickDropButtonDown -= DropWeaponFromHand;
        _objectPicker.OnPickedUpObject -= TryEquipWeapon;

    }
}

[System.Serializable]
public class PlayerWeapons
{
    public Weapon weapon;
    public EHandType handType;
    public Transform weaponIKParent;
    public Transform weaponParent;
    public TwoBoneIKConstraint handIK;
}
