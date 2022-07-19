using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStone : Weapon
{
    [SerializeField] private float _throwForce = 5;

    public override void Shoot(Vector3 point)
    {
        base.Shoot(point);

        DamageData damageData = new DamageData();
        damageData.owner = _owner;
        damageData.damage = _damage;

        var createdProjectile = Instantiate(_projectilePrefab, _muzzle.transform.position, Quaternion.LookRotation(_muzzle.transform.forward));

        createdProjectile.SetDamageData(damageData);

        createdProjectile.transform.rotation = Quaternion.LookRotation(point);

        Vector3 projectileDirection = point - _muzzle.transform.position;
        createdProjectile.SetDirection(projectileDirection.normalized * _throwForce);
    }
}
