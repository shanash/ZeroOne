using UnityEngine;

public class Background : MonoBehaviour, ICursorInteractable, FluffyDuck.Util.MonoFactory.IProduct
{
    bool Initialize(Me_Resource_Data data)
    {
        return true;
    }
}
