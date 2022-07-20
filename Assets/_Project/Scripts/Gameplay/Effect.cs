using UnityEngine;

public abstract class Effect
{
    public int currentValue;
    public int maxValue;
    public IDamageable target;
    public ScriptableEffect effect { get; }



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
            Debug.Log($"Effect {effect.effectName} increased by {value} ");
        }
        CheckEffects();
    }

    public virtual void CheckEffects()
    {

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

    public virtual void Decrease(int value)
    {
        currentValue -= value;

        if (currentValue <= 0)
        {
            currentValue = 0;
            Deactivate();
        }
    }
}
