using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pawn : MonoBehaviour, IDamageable
{
    public float Health => _health;
    public bool IsDead => _isDead;
    public Transform HeadBone => _headBone;
    public Renderer Skin => _skin;

    [SerializeField] private float _maxHealth;
    [SerializeField] private float _health;
    [SerializeField] private Transform _headBone;
    [SerializeField] private Renderer _skin;
    private bool _isDead;

    public Action OnDeath;
    public Action<DamageData> OnTakedDamage;
    public Action<Effect> OnEffectValueChanged;

    private Dictionary<ScriptableTimedEffect, TimedEffect> _timedEffects = new Dictionary<ScriptableTimedEffect, TimedEffect>();
    private Dictionary<ScriptableEffect, Effect> _effects = new Dictionary<ScriptableEffect, Effect>();


    public virtual void Awake()
    {
        ResetHealth();
    }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        EffectsTimeTick();
    }

    public virtual void TakeDamage(DamageData damageData)
    {
        if (IsDead) return;

        _health -= damageData.damage;
        OnTakedDamage?.Invoke(damageData);

        if (_health <= 0)
        {
            _health = 0;
            _isDead = true;
            Death(damageData);
        }
    }

    void ResetHealth()
    {
        _health = _maxHealth;
    }

    public virtual void Death(DamageData damageData)
    {
        OnDeath?.Invoke();
    }

    public float GetHealth01()
    {
        if (_maxHealth == 0 || _health == 0)
            return 0;

        float health01 = Mathf.Clamp(_health / _maxHealth, 0f, 1f);
        return health01;
    }

    public void AddTimedEffect(TimedEffect effect)
    {
        if (_timedEffects.ContainsKey(effect.Effect))
        {
            _timedEffects[effect.Effect].Activate();
        }
        else
        {
            _timedEffects.Add(effect.Effect, effect);
            effect.Activate();
        }
    }

    public void RemoveTimedEffect(TimedEffect timedEffect)
    {
        if (timedEffect == null) return;
        if (_timedEffects.ContainsKey(timedEffect.Effect) == false)
            return;

        _timedEffects[timedEffect.Effect].End();
        _timedEffects.Remove(timedEffect.Effect);
    }

    public TimedEffect GetTimedEffect(ScriptableTimedEffect scriptableTimedEffect)
    {
        if (_timedEffects.ContainsKey(scriptableTimedEffect) == false)
            return null;

        return _timedEffects.FirstOrDefault(te => te.Key == scriptableTimedEffect).Value;
    }

    public void EffectsTimeTick()
    {
        if (_timedEffects.Count <= 0) return;

        for (int i = 0; i < _timedEffects.Values.Count; i++)
        {
            var item = _timedEffects.ElementAt(i);
            var effect = item.Value;

            if (IsDead)
            {
                effect.IsFinished = true;
                effect.End();
            }

            effect.Tick(Time.deltaTime);

            if (item.Key.unlimitedDuration)
                continue;

            if (effect.IsFinished)
                _timedEffects.Remove(effect.Effect);
        }
    }


    public List<ScriptableTimedEffect> GetTimedEffects()
    {
        return _timedEffects.Keys.ToList();
    }

    public List<ScriptableEffect> GetEffects()
    {
        return _effects.Keys.ToList();
    }

    public void AddEffect(Effect newEffect)
    {
        if (newEffect == null) return;

        if (_effects.ContainsKey(newEffect.effect))
        {
            // сами в себя кастанем значение
            _effects[newEffect.effect].Increase(newEffect.currentValue);
        }
        else
        {
            _effects.Add(newEffect.effect, newEffect);
            newEffect.OnValueChanged += OnEffectValueChanged;
            newEffect.Initialize();
        }


        foreach (var effect in _effects)
        {
            OnEffectValueChanged?.Invoke(effect.Value);
        }

    }

    public void RemoveEffect(ScriptableEffect effect)
    {
        if (!_effects.ContainsKey(effect)) return;

        _effects[effect].Deactivate();
    }

    public void RemoveEffect(Effect effect)
    {
        if (!_effects.ContainsValue(effect)) return;

        effect.OnValueChanged -= OnEffectValueChanged;
        _effects.Remove(effect.effect);
    }


    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public Effect GetEffect(ScriptableEffect effect)
    {
        if (_effects.ContainsKey(effect) == false)
            return null;

        return _effects.FirstOrDefault(te => te.Key == effect).Value;
    }

}
