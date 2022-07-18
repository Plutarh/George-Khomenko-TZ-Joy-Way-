using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IPickable
{
    public bool IsPicked => _isPicked;
    public bool IsIK => _isIK;

    [SerializeField] private Transform _muzzle;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private float _damage;
    [SerializeField] private float _projectileMoveSpeed;
    [SerializeField] private GameObject _owner;
    [SerializeField] private List<ScriptableEffect> _effectsOnHit = new List<ScriptableEffect>();

    [SerializeField] private string _weaponName;
    [SerializeField] private bool _isIK;

    private bool _isPicked;


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

    public void PickUp(GameObject owner)
    {
        _owner = owner;
        _isPicked = true;
    }

    public void Drop()
    {
        _isPicked = false;
        _owner = null;

        gameObject.AddComponent<PickableObject>();
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

