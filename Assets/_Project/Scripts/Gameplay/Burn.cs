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


    public override void Deactivate()
    {
        base.Deactivate();

        if (target != null)
            target.RemoveTimedEffect(target.GetTimedEffect(_burnScriptableEffect.scriptableTimedEffect));
    }

}