using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HitEffects/BurnEffect")]
public class BurnScriptableEffect : ScriptableEffect
{
    public float timerToHit;
    public float damage;

    public ParticleSystem burnFX;

    public override TimedEffect InitializeEffect(GameObject obj, DamageData combatData)
    {
        return new BurnTimedEffect(this, obj, combatData);
    }

    public override TimedEffect InitializeEffect(GameObject obj)
    {
        return new BurnTimedEffect(this, obj);
    }
}
