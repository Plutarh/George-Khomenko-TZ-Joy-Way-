using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected GameObject _hitFX;
    [SerializeField] protected List<ScriptableTimedEffect> _timedEffectOnHit = new List<ScriptableTimedEffect>();
    [SerializeField] private List<EffectsInteractions> _effectsInteractions = new List<EffectsInteractions>();

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

    public void AddEffectInteraction(EffectsInteractions effectsInteractions)
    {
        _effectsInteractions.Add(effectsInteractions);
    }

    public void AddScriptableTimedEffect(ScriptableTimedEffect newEffect)
    {
        _timedEffectOnHit.Add(newEffect);
    }

    public virtual void Hit(IDamageable damageable)
    {
        foreach (var ef in _effectsInteractions)
        {

            var timedEffect = damageable.GetTimedEffect(ef.timedEffect);
            if (timedEffect == null) continue;

            switch (ef.eEfectInrecation)
            {
                case EffectsInteractions.EEfectInrecation.None:
                    break;
                case EffectsInteractions.EEfectInrecation.DecreaseDamage:
                    _damageData.damage -= ef.damage;
                    break;
                case EffectsInteractions.EEfectInrecation.IncreaseDamage:
                    _damageData.damage += ef.damage;
                    break;
                case EffectsInteractions.EEfectInrecation.RefreshEffect:
                    timedEffect.Reactivate();
                    break;
                case EffectsInteractions.EEfectInrecation.StopEffect:

                    damageable.RemoveTimedEffect(timedEffect);
                    break;
            }
        }

        _timedEffectOnHit.ForEach(effect => damageable.AddTimedEffect(effect.InitializeEffect(damageable.GetGameObject(), _damageData)));
        damageable.TakeDamage(_damageData);
    }
}
