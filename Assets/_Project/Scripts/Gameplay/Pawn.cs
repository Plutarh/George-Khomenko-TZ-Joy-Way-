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

    Dictionary<ScriptableEffect, TimedEffect> _timedEffects = new Dictionary<ScriptableEffect, TimedEffect>();

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

    public void AddEffect(TimedEffect effect)
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

    public void RemoveEffect(TimedEffect effect)
    {
        if (_timedEffects.ContainsKey(effect.Effect) == false)
            return;

        _timedEffects[effect.Effect].End();
        _timedEffects.Remove(effect.Effect);
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

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public List<ScriptableEffect> GetTimedEffects()
    {
        return _timedEffects.Keys.ToList();
    }
}
