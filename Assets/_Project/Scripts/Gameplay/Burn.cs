public class Burn : Effect
{
    BurnScriptableEffect _burnScriptableEffect;

    public Burn(ScriptableEffect ef) : base(ef)
    {
        _burnScriptableEffect = (BurnScriptableEffect)effect;
        maxValue = _burnScriptableEffect.maxValue;
    }

    public override void Activate()
    {
        base.Activate();
        target.AddTimedEffect(_burnScriptableEffect.scriptableTimedEffect.InitializeEffect(target.GetGameObject()));
    }

    public override void CheckEffects()
    {
        base.CheckEffects();


        var effect = target.GetEffect(_burnScriptableEffect.conterEffect);

        if (effect == null) return;
        effect.Decrease(currentValue);

    }

    public override void Deactivate()
    {
        base.Deactivate();
        target.RemoveTimedEffect(target.GetTimedEffect(_burnScriptableEffect.scriptableTimedEffect));
    }

}