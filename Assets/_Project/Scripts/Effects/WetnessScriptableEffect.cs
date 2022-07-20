using UnityEngine;

[CreateAssetMenu(menuName = "Effects/WetnessEffect")]
public class WetnessScriptableEffect : ScriptableEffect
{
    public override Effect InitializeEffect()
    {
        return new Wetness(this);
    }
}

