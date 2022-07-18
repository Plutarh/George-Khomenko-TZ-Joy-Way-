using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(DamageData damageData);
    void AddEffect(TimedEffect timedEffect);
    List<ScriptableEffect> GetTimedEffects();
    GameObject GetGameObject();
}
