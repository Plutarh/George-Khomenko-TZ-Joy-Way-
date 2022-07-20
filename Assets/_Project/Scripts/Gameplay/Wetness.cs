public class Wetness : Effect
{
    WetnessScriptableEffect _wetnessScriptableEffect;

    public Wetness(ScriptableEffect ef) : base(ef)
    {
        _wetnessScriptableEffect = (WetnessScriptableEffect)effect;
        maxValue = _wetnessScriptableEffect.maxValue;
    }

    public override void Activate()
    {
        base.Activate();
        target.AddTimedEffect(_wetnessScriptableEffect.scriptableTimedEffect.InitializeEffect(target.GetGameObject()));
    }

    public override void CheckEffects()
    {
        base.CheckEffects();

        if (target.GetEffects().Contains(_wetnessScriptableEffect.conterEffect))
        {
            target.RemoveEffect(_wetnessScriptableEffect.conterEffect);
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        target.RemoveTimedEffect(target.GetTimedEffect(_wetnessScriptableEffect.scriptableTimedEffect));
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();

        target.GetEffects();
    }

}
