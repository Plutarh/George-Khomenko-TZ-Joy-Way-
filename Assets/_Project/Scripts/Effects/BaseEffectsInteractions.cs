[System.Serializable]
public class BaseEffectsInteractions
{
    public ScriptableEffect effect;
    public EInteractionType interactionType;


    public enum EInteractionType
    {
        None,
        DecreaseByOwnValue,
        IncreaseByOwnValue,
    }
}
