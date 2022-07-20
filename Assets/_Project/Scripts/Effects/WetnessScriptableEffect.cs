using UnityEngine;

[CreateAssetMenu(menuName = "Effects/WetnessEffect")]
public class WetnessScriptableEffect : ScriptableEffect
{
    public override Effect InitializeEffect(GameObject obj)
    {
        return new Wetness(obj);
    }

}
