using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleProjectile : Projectile
{
    [SerializeField] private ParticleSystem _damageParticle;
    [SerializeField] private List<ParticleSystem> _allParticles = new List<ParticleSystem>();

    private List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();

    public override void Awake()
    {
        base.Awake();
        _allParticles = GetComponentsInChildren<ParticleSystem>().ToList();
    }

    public void Emit(bool state)
    {
        foreach (var particle in _allParticles)
        {
            var emission = particle.emission;
            emission.enabled = state;
        }
    }

    public override void SetDirection(Vector3 direction)
    {
        base.SetDirection(direction);
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.transform.root == _owner.transform.root) return;

        int numCollisionEvents = _damageParticle.GetCollisionEvents(other, _collisionEvents);

        var damagable = other.transform.root.GetComponent<IDamageable>();

        if (damagable == null) return;
        int i = 0;

        while (i < numCollisionEvents)
        {
            DamageData damageData = new DamageData();

            damageData.damage = 1;

            damagable.TakeDamage(damageData);
            i++;
        }
    }


}
