using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected GameObject _hitFX;
    [SerializeField] private List<EffectsInteractions> _timedEffectsInteraction = new List<EffectsInteractions>();
    [SerializeField] protected List<Effect> _effects = new List<Effect>();

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
        _timedEffectsInteraction.Add(effectsInteractions);
    }

    public void AddEffect(Effect newEffect)
    {
        if (_effects.Any(ef => ef.effect == newEffect.effect))
            _effects.Remove(_effects.FirstOrDefault(ef => ef.effect == newEffect.effect));

        _effects.Add(newEffect);
    }

    void ResolveTimeEffectsInteractions(IDamageable damageable)
    {
        foreach (var ef in _timedEffectsInteraction)
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
    }

    void ResolveEffectsInteractions(IDamageable damageable)
    {
        foreach (var effect in _effects)
        {
            if (effect.effect.interactions.Count == 0)
            {
                effect.AddTarget(damageable);
                damageable.AddEffect(effect);
            }
            else
            {
                foreach (var interaction in effect.effect.interactions)
                {
                    var effectOnDamagable = damageable.GetEffect(interaction.effect);
                    if (effectOnDamagable == null) continue;

                    switch (interaction.interactionType)
                    {
                        case BaseEffectsInteractions.EInteractionType.None:
                            break;
                        case BaseEffectsInteractions.EInteractionType.DecreaseByOwnValue:

                            effectOnDamagable.Decrease(effect.currentValue);
                            effect.Decrease(effectOnDamagable.currentValue);

                            break;
                        case BaseEffectsInteractions.EInteractionType.IncreaseByOwnValue:
                            effectOnDamagable.Increase(effect.currentValue);
                            break;
                    }
                }

                if (effect.currentValue > 0)
                {
                    effect.AddTarget(damageable);
                    damageable.AddEffect(effect);
                }
            }
        }
    }

    public virtual void Hit(IDamageable damageable)
    {
        ResolveTimeEffectsInteractions(damageable);
        ResolveEffectsInteractions(damageable);
        damageable.TakeDamage(_damageData);
    }
}
