using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyProjectile : Projectile
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _timeToEnableGravity;

    private Rigidbody _body;
    private bool _isHitted;

    public override void Awake()
    {
        base.Awake();
        _body = GetComponent<Rigidbody>();
    }

    public override void Update()
    {
        base.Update();

        _lifeTime -= Time.deltaTime;

        if (_lifeTime <= 0)
            DestroyProjectile();
    }

    public override void SetDirection(Vector3 direction)
    {
        base.SetDirection(direction);
        _body.AddForce(direction, ForceMode.Impulse);
    }

    void DestroyProjectile()
    {
        var hitFX = Instantiate(_hitFX, transform.position + (-transform.forward), Quaternion.identity);
        Destroy(hitFX.gameObject, 2f);
        Destroy(gameObject);
    }

    public override void Hit(IDamageable damageable)
    {
        base.Hit(damageable);
        _damageData.velocity = _body.velocity;
        damageable.TakeDamage(_damageData);

        foreach (var item in damageable.GetTimedEffects())
        {
            Debug.LogError($"{item.name}");
        }

        _effectsOnHit.ForEach(effect => damageable.AddEffect(effect.InitializeEffect(damageable.GetGameObject(), _damageData)));

        DestroyProjectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isHitted) return;
        var damageable = other.transform.root.GetComponent<IDamageable>();

        if (damageable == null)
        {
            DestroyProjectile();
            return;
        }

        Hit(damageable);
        _isHitted = true;
    }

}