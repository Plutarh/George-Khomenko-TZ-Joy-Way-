using UnityEngine;

[CreateAssetMenu(menuName = "HitEffects/WetnessEffect")]
public class WetnessScriptableEffect : ScriptableEffect
{
    public ParticleSystem wetnessFX;

    public override TimedEffect InitializeEffect(GameObject obj, DamageData combatData)
    {
        return new BurnTimedEffect(this, obj, combatData);
    }

    public override TimedEffect InitializeEffect(GameObject obj)
    {
        return new BurnTimedEffect(this, obj);
    }
}
