using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(DamageData damageData);

    void AddTimedEffect(TimedEffect timedEffect);
    void RemoveTimedEffect(TimedEffect timedEffect);
    List<ScriptableTimedEffect> GetTimedEffects();
    TimedEffect GetTimedEffect(ScriptableTimedEffect scriptableTimedEffect);

    List<ScriptableEffect> GetEffects();
    void AddEffect(Effect effect);
    void RemoveEffect(ScriptableEffect effect);
    void RemoveEffect(Effect effect);
    Effect GetEffect(ScriptableEffect effect);

    GameObject GetGameObject();
}
