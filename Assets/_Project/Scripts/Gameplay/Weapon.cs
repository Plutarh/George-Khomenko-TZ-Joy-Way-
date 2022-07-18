using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private float _damage;
    [SerializeField] private float _projectileMoveSpeed;
    [SerializeField] private GameObject _owner;
    [SerializeField] private List<ScriptableEffect> _effectsOnHit = new List<ScriptableEffect>();

    void Start()
    {

    }

    void Update()
    {

    }

    public Weapon PickUp(GameObject owner)
    {
        _owner = owner;
        return this;
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
}

public interface IWeapon
{
    void Shoot();
    string GetAnimationName();
}
