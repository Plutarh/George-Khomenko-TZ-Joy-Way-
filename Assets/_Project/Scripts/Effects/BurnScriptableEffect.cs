using UnityEngine;

[CreateAssetMenu(menuName = "Effects/BurnEffect")]
public class BurnScriptableEffect : ScriptableEffect
{
    public override Effect InitializeEffect()
    {
        return new Burn(this);
    }
}

