using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Weapon
{
    private ParticleProjectile _flameThrower;
    [SerializeField] private int _flameDamage = 1;

    public override void Awake()
    {
        base.Awake();

        _flameThrower = Instantiate(_projectilePrefab, _muzzle.transform) as ParticleProjectile;
        _flameThrower.transform.localPosition = Vector3.zero;
        _flameThrower.Emit(false);
    }

    public override void Shoot(Vector3 point)
    {
        base.Shoot(point);

        DamageData damageData = new DamageData();
        damageData.owner = _owner;
        damageData.damage = _damage;

        _flameThrower.SetDamageData(damageData);

        _flameThrower.transform.rotation = Quaternion.LookRotation(point);

        Vector3 projectileDirection = point - _muzzle.transform.position;
        _flameThrower.SetDirection(projectileDirection.normalized);

        _effectsInteractions.ForEach(ef => _flameThrower.AddEffectInteraction(ef));

        foreach (var effect in _effects)
        {
            var newEffect = effect.InitializeEffect();
            newEffect.currentValue = _flameDamage;
            _flameThrower.AddEffect(newEffect);
        }


        _flameThrower.Emit(true);
    }

    public override void StopShoot()
    {
        base.StopShoot();

        _flameThrower.Emit(false);
    }

}
