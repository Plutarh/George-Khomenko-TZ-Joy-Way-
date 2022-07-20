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

        var damagable = other.transform.root.GetComponent<IDamageable>();

        if (damagable == null) return;

        Hit(damagable);
    }

    public override void Hit(IDamageable damageable)
    {
        base.Hit(damageable);
    }
}
