using UnityEngine;

[CreateAssetMenu(menuName = "HitEffects/WetnessEffect")]
public class WetnessScriptableEffect : ScriptableTimedEffect
{
    public ParticleSystem wetnessFX;

    public override TimedEffect InitializeEffect(GameObject obj, DamageData combatData)
    {
        return new WetnessTimedEffect(this, obj, combatData);
    }

    public override TimedEffect InitializeEffect(GameObject obj)
    {
        return new WetnessTimedEffect(this, obj);
    }
}
