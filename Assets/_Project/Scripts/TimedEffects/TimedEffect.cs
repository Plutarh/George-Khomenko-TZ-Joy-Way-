using UnityEngine;

public abstract class TimedEffect
{
    internal float currentDuration;
    public bool unlimitedDuration = false;
    internal float totalDuration;
    public ScriptableTimedEffect Effect { get; }
    protected readonly GameObject targetGO;
    protected readonly GameObject whoUsed;

    public bool IsFinished;
    public bool IsPaused;

    public DamageData damageData;

    public TimedEffect(ScriptableTimedEffect effect, GameObject targetObj, DamageData combat)
    {
        Effect = effect;
        targetGO = targetObj;
        whoUsed = combat.owner;
        damageData = combat;

    }

    public TimedEffect(ScriptableTimedEffect effect, GameObject targetObj)
    {
        Effect = effect;
        targetGO = targetObj;
    }

    public virtual void Tick(float delta)
    {
        if (Effect.unlimitedDuration)
            return;
        if (totalDuration <= 0)
            totalDuration = currentDuration;

        currentDuration -= delta;

        if (currentDuration <= 0)
        {
            End();
            IsFinished = true;
            IsPaused = false;
        }
    }

    public void Activate()
    {
        if (currentDuration <= 0)
        {
            ApplyEffect();
            currentDuration += Effect.duration;
        }

        if (Effect.isDurationRefreshed && currentDuration > 0)
        {
            currentDuration = Effect.duration;
        }
    }

    public virtual void Reactivate()
    {

    }

    protected abstract void ApplyEffect();
    public abstract void End();
}
