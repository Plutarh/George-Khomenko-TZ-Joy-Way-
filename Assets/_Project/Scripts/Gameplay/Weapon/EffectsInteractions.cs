[System.Serializable]
public class EffectsInteractions
{
    public ScriptableTimedEffect timedEffect;
    public int damage;
    public EEfectInrecation eEfectInrecation;

    public enum EEfectInrecation
    {
        None,
        DecreaseDamage,
        IncreaseDamage,
        RefreshEffect,
        StopEffect
    }
}