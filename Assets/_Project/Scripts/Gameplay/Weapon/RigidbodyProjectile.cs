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

        if (_body == null)
            _body = gameObject.AddComponent<Rigidbody>();

        _body.useGravity = false;
    }

    public override void Update()
    {
        base.Update();

        _lifeTime -= Time.deltaTime;

        if (_lifeTime <= 0)
            DestroyProjectile();

        _timeToEnableGravity -= Time.deltaTime;

        if (_timeToEnableGravity <= 0)
            _body.useGravity = true;
    }

    public override void SetDirection(Vector3 direction)
    {
        base.SetDirection(direction);
        _body.AddForce(direction, ForceMode.VelocityChange);
    }

    void DestroyProjectile()
    {
        var hitFX = Instantiate(_hitFX, transform.position + (-transform.forward), Quaternion.identity);
        Destroy(hitFX.gameObject, 2f);
        Destroy(gameObject);
    }

    public override void Hit(IDamageable damageable)
    {
        _damageData.velocity = _body.velocity;
        base.Hit(damageable);


        DestroyProjectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isHitted) return;
        if (other.transform.root == _owner.transform.root) return;

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
