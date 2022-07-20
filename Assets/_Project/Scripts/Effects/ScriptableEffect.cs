using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableEffect : ScriptableObject
{
    public string effectName;
    public int maxValue;
    public ScriptableTimedEffect scriptableTimedEffect;
    public ScriptableEffect conterEffect;
    public abstract Effect InitializeEffect();
}

public class BaseEffectsInteractions
{
    public ScriptableEffect effect;



}
