using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableEffect : ScriptableObject
{
    public abstract Effect InitializeEffect(GameObject target);
}
