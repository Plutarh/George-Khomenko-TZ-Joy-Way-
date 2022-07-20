using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(DamageData damageData);
    void AddTimedEffect(TimedEffect timedEffect);
    void RemoveTimedEffect(TimedEffect timedEffect);
    List<ScriptableTimedEffect> GetTimedEffects();
    TimedEffect GetTimedEffect(ScriptableTimedEffect scriptableTimedEffect);
    List<Effect> GetEffects();
    GameObject GetGameObject();
}
