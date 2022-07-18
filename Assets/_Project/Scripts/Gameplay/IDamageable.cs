using UnityEngine;

public interface IDamageable
{
    void TakeDamage(DamageData damageData);
    void AddEffect(TimedEffect timedEffect);
    GameObject GetGameObject();
}
