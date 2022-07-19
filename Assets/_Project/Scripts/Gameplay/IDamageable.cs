using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(DamageData damageData);
    void AddTimedEffect(TimedEffect timedEffect);
    List<ScriptableTimedEffect> GetTimedEffects();
    List<Effect> GetEffects();
    GameObject GetGameObject();
}
