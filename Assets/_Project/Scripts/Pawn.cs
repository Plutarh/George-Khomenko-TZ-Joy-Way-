using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour, IDamageable
{
    public float Health => _health;
    public bool IsDead => _isDead;
    public Transform HeadBone => _headBone;


    [SerializeField] private float _maxHealth;
    [SerializeField] private float _health;
    [SerializeField] private Transform _headBone;

    private bool _isDead;

    public Action OnDeath;
    public Action<DamageData> OnTakedDamage;

    public virtual void Awake()
    {
        ResetHealth();
    }

    public virtual void Start()
    {

    }


    public virtual void Update()
    {

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
            Death();
        }
    }

    void ResetHealth()
    {
        _health = _maxHealth;
    }

    public virtual void Death()
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
}
