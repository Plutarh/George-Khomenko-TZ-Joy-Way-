using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool IsPicked => _isPicked;

    [SerializeField] private Transform _muzzle;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private float _damage;
    [SerializeField] private float _projectileMoveSpeed;
    [SerializeField] private GameObject _owner;
    [SerializeField] private List<ScriptableEffect> _effectsOnHit = new List<ScriptableEffect>();

    [SerializeField] private string _weaponName;

    private bool _isPicked;

    private FloatingObject _floatingObject;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_weaponName == string.Empty)
        {
            _weaponName = name;
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }

#endif
    void Start()
    {

    }

    void Update()
    {

    }

    public Weapon PickUp(GameObject owner)
    {
        _owner = owner;
        _isPicked = true;

        if (_floatingObject)
            Destroy(_floatingObject);

        return this;
    }

    public void Drop()
    {
        _isPicked = false;
        _owner = null;

        if (_floatingObject == null)
            _floatingObject = gameObject.AddComponent<FloatingObject>();
    }

    public void Shoot(Vector3 direction)
    {
        var createdProjectile = Instantiate(_projectilePrefab, _muzzle.transform.position, Quaternion.LookRotation(_muzzle.transform.forward));

        DamageData damageData = new DamageData();
        damageData.damage = _damage;
        damageData.owner = _owner;
        createdProjectile.SetDamageData(damageData);
        _effectsOnHit.ForEach(effect => createdProjectile.AddScriptableEffect(effect));

        Vector3 projectileDirection = direction - _muzzle.transform.position;
        createdProjectile.SetMoveDirection(projectileDirection.normalized * _projectileMoveSpeed);
    }

    private void OnDestroy()
    {
        GlobalEvents.OnWeaponDestroyed?.Invoke(_weaponName);
    }
}

