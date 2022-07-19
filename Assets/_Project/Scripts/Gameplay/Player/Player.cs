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

    [SerializeField] private List<PlayerHand> _playerWeapons = new List<PlayerHand>();

    private PlayerMover _playerMover;
    private ObjectPicker _objectPicker;
    private Animator _animator;

    private string _currentCombatName;

    [SerializeField] private List<Weapon> _delayedWeaponToShoot = new List<Weapon>();

    private float _throwAnimationDelay = 0.3f;
    private float _throwAnimationTimeout;

    public override void Awake()
    {
        base.Awake();

        _mainCamera = Camera.main;
        _playerMover = GetComponent<PlayerMover>();
        _objectPicker = GetComponent<ObjectPicker>();
        _animator = GetComponent<Animator>();

        InputService.OnAttackButtonPressed += TryToAttack;
        InputService.OnAttackButtonUp += TryToStopAttack;
        InputService.OnHandPickDropButtonDown += PickDropWithHand;
        _objectPicker.OnPickedUpObject += TryEquipWeapon;

        _playerWeapons.ForEach(pw => ResetHandIK(pw.handIK));
    }

    public override void Update()
    {
        base.Update();
    }

    void TryToStopAttack(EHandType handType)
    {
        var foundedHand = GetHandByType(handType);

        var weaponInHand = foundedHand.weapon;

        if (weaponInHand == null)
            return;

        weaponInHand.StopShoot();
    }



    void TryToAttack(EHandType handType)
    {
        var foundedHand = GetHandByType(handType);

        var weaponInHand = foundedHand.weapon;

        if (weaponInHand == null)
            return;

        _playerMover.battleState = true;

        if (weaponInHand.IsAnimationRequire)
        {
            if (GetAttackLayerAnimationTime() >= 0.7f && Time.time > _throwAnimationTimeout)
            {
                _throwAnimationTimeout = Time.time + _throwAnimationDelay;
                string animationName = weaponInHand.AnimationName + (handType == EHandType.Left ? "_M" : string.Empty);
                _delayedWeaponToShoot.Add(weaponInHand);
                _animator.CrossFade(animationName, 0.1f, 1);
                Debug.Log($"Player animation {animationName}");
                _currentCombatName = animationName;

            }
        }
        else
        {
            Attack(weaponInHand);
        }
    }

    void Throw()
    {
        if (_delayedWeaponToShoot.Count == 0) return;

        var weaponToShoot = _delayedWeaponToShoot.FirstOrDefault();

        if (weaponToShoot == null) return;

        weaponToShoot.Shoot(GetAimPoint());
        _delayedWeaponToShoot.RemoveAt(0);
    }

    float GetAttackLayerAnimationTime()
    {
        if (_animator.GetCurrentAnimatorStateInfo(1).IsName(_currentCombatName))
            return _animator.GetCurrentAnimatorStateInfo(1).normalizedTime;
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName(_currentCombatName))
            return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        else
            return 1;
    }



    void Attack(Weapon weapon)
    {
        weapon.Shoot(GetAimPoint());
    }

    PlayerHand GetHandByType(EHandType handType)
    {
        var foundedHand = _playerWeapons.FirstOrDefault(pw => pw.handType == handType);

        if (foundedHand == null)
        {
            Debug.LogError($"Cannot find hand : {handType}");
            return null;
        }

        return foundedHand;
    }

    void PickDropWithHand(EHandType handType)
    {
        var foundedHand = GetHandByType(handType);

        var weaponInHand = foundedHand.weapon;

        if (weaponInHand == null)
        {
            _objectPicker.PickUpClosestObject(handType);
        }
        else
        {
            DropWeaponFromHand(handType);
        }
    }

    void DropWeaponFromHand(EHandType handType)
    {
        var foundedHand = GetHandByType(handType);

        var weaponInHand = foundedHand.weapon;

        if (weaponInHand == null)
        {
            Debug.LogError($"Cannot find weapon in hand : {handType}");
            return;
        }

        weaponInHand.transform.SetParent(null);
        weaponInHand.Drop();

        if (weaponInHand.IsIKRequire)
            ResetHandIK(foundedHand.handIK);

        foundedHand.weapon = null;
    }

    void TryEquipWeapon(IPickable pickable, EHandType handType)
    {
        if (!(pickable is Weapon)) return;
        EquipWeapon(pickable as Weapon, handType);
    }

    void EquipWeapon(Weapon weapon, EHandType handType)
    {
        var foundedHand = GetHandByType(handType);

        // В ТЗ нет подрбонго описания, что делать если пытаемся подобрать в руку предмет, если в ней уже находится что-то
        if (foundedHand.weapon != null)
        {
            //Ничего не делаем
            return;

            // Дропаем старое оружие
            // foundedHand.weapon.Drop();
        }


        foundedHand.weapon = weapon;

        if (weapon.IsIKRequire)
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
    }

    void SmoothEnableHandIK(TwoBoneIKConstraint iKConstraint)
    {
        iKConstraint.weight = 1;
    }

    void ResetHandIK(TwoBoneIKConstraint iKConstraint)
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

    private void OnDestroy()
    {
        InputService.OnAttackButtonPressed -= TryToAttack;
        InputService.OnAttackButtonUp -= TryToStopAttack;
        InputService.OnHandPickDropButtonDown -= PickDropWithHand;
        _objectPicker.OnPickedUpObject -= TryEquipWeapon;

    }
}

[System.Serializable]
public class PlayerHand
{
    public Weapon weapon;
    public EHandType handType;
    public Transform weaponIKParent;
    public Transform weaponParent;
    public TwoBoneIKConstraint handIK;
}
