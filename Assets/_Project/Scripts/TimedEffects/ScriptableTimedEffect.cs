using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableTimedEffect : ScriptableObject
{
    public float duration;
    public bool unlimitedDuration = false;
    public bool isDurationRefreshed = true;
    public bool isStackable = false;

    public abstract TimedEffect InitializeEffect(GameObject target, DamageData damageData);
    public abstract TimedEffect InitializeEffect(GameObject target);
}
