using UnityEngine;

public class StimuliSource : MonoBehaviour
{
    public enum SourceSide
    {
        PlayerFriends,
        Enemies,
        Neutrals
    }

    public int Priority;
    public SourceSide Side;
}
