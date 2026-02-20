using UnityEngine;

public class Drop : MonoBehaviour
{
    public DropDefinition DropDefinition;

    internal void NotifyPickedUp()
    {
        Destroy(gameObject);
    }
}
