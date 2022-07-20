using System;
using UnityEngine;

public abstract class Effect
{
    public int currentValue;
    public int maxValue;
    public IDamageable target;
    public ScriptableEffect effect { get; }

    public Action<Effect> OnValueChanged;

    public Effect(ScriptableEffect ef)
    {
        effect = ef;
    }

    public void AddTarget(IDamageable newTarget)
    {
        target = newTarget;
    }

    public virtual void Increase(int value)
    {
        currentValue += value;

        if (currentValue >= maxValue)
        {
            currentValue = maxValue;
            Activate();
        }

        OnValueChanged?.Invoke(this);
    }

    public virtual void Decrease(int value)
    {
        currentValue -= value;

        if (currentValue <= 0)
        {
            currentValue = 0;
            if (target != null)
                target.RemoveEffect(this);
        }


        if (currentValue < maxValue)
            Deactivate();

        OnValueChanged?.Invoke(this);
    }

    public virtual void Initialize()
    {
        if (currentValue >= maxValue)
        {
            currentValue = maxValue;
            Activate();
        }
    }

    public virtual void Activate()
    {

    }

    public virtual void Deactivate()
    {

    }

    public virtual void ApplyEffect()
    {

    }

    public float GetValue01()
    {
        if (maxValue == 0 || currentValue == 0)
            return 0;

        float value01 = Mathf.Clamp((float)currentValue / (float)maxValue, 0f, 1f);
        return value01;
    }

}
