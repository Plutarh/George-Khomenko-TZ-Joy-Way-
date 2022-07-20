using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] private float _projectileMoveSpeed;
    [SerializeField] private float _shotPerSecond = 0.1f;

    private float _shotTimeout;

    public override void Shoot(Vector3 point)
    {
        if (Time.time < _shotTimeout) return;

        _shotTimeout = Time.time + _shotPerSecond;

        base.Shoot(point);

        var createdProjectile = Instantiate(_projectilePrefab, _muzzle.transform.position, Quaternion.LookRotation(_muzzle.transform.forward));

        DamageData damageData = new DamageData();
        damageData.damage = _damage;
        damageData.owner = _owner;
        createdProjectile.SetDamageData(damageData);

        _effectsInteractions.ForEach(ef => createdProjectile.AddEffectInteraction(ef));
        _timedEffectOnHit.ForEach(effect => createdProjectile.AddScriptableTimedEffect(effect));

        Vector3 projectileDirection = point - _muzzle.transform.position;
        createdProjectile.SetDirection(projectileDirection.normalized * _projectileMoveSpeed);
    }
}
