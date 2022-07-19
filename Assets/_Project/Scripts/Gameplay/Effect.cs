using UnityEngine;

[System.Serializable]
public abstract class Effect
{
    public GameObject targetGO;

    public Effect(GameObject go)
    {
        targetGO = go;
    }
}

[System.Serializable]
public class Wetness : Effect
{
    public int wetness = 0;
    const int maxWetness = 100;

    public Wetness(GameObject targetGo) : base(targetGo)
    {
        wetness = 0;
    }

    public void AddWetness(int value)
    {
        wetness += value;

        if (wetness > maxWetness)
            wetness = maxWetness;
    }

    public void DecreaseWetness(int value)
    {
        wetness -= value;

        if (wetness < 0)
            wetness = 0;
    }
}