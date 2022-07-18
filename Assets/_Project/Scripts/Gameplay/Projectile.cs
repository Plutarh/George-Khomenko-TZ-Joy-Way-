using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject _hitFX;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _timeToEnableGravity;
    [SerializeField] private List<ScriptableEffect> _effectsOnHit = new List<ScriptableEffect>();
    private Rigidbody _body;
    private DamageData _damageData;
    private bool _isHitted;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _lifeTime -= Time.deltaTime;

        if (_lifeTime <= 0)
            DestroyProjectile();
    }

    public void SetDamageData(DamageData damageData)
    {
        _damageData = damageData;
    }

    public void SetMoveDirection(Vector3 direction)
    {
        _body.AddForce(direction, ForceMode.Impulse);
        transform.rotation = Quaternion.LookRotation(direction.normalized);
    }

    public void AddScriptableEffect(ScriptableEffect newEffect)
    {
        _effectsOnHit.Add(newEffect);
    }

    void DestroyProjectile()
    {
        var hitFX = Instantiate(_hitFX, transform.position + (-transform.forward), Quaternion.identity);
        Destroy(hitFX.gameObject, 2f);
        Destroy(gameObject);
    }

    void Hit(IDamageable damageable)
    {
        _damageData.velocity = _body.velocity;
        damageable.TakeDamage(_damageData);

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
