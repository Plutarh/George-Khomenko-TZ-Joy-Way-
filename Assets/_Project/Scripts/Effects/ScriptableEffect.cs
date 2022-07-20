using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableEffect : ScriptableObject
{
    public string effectName;
    public int maxValue;
    public ScriptableTimedEffect scriptableTimedEffect;

    public List<BaseEffectsInteractions> interactions = new List<BaseEffectsInteractions>();
    public abstract Effect InitializeEffect();
}
