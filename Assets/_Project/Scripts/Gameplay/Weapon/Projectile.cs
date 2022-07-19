using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected GameObject _hitFX;
    [SerializeField] protected List<ScriptableEffect> _effectsOnHit = new List<ScriptableEffect>();

    protected DamageData _damageData;
    protected GameObject _owner;

    public virtual void Awake()
    {

    }

    public virtual void Update()
    {

    }

    public void SetDamageData(DamageData damageData)
    {
        _damageData = damageData;
        _owner = _damageData.owner;
    }

    public virtual void SetDirection(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction.normalized);
    }

    public void AddScriptableEffect(ScriptableEffect newEffect)
    {
        _effectsOnHit.Add(newEffect);
    }

    public virtual void Hit(IDamageable damageable)
    {

    }
}
