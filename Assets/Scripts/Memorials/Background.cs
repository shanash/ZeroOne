using UnityEngine;
using ZeroOne.Input;

public class Background : MonoBehaviour, ICursorInteractable, FluffyDuck.Util.MonoFactory.IProduct
{
    bool Initialize(L2d_Char_Skin_Data data)
    {
        return true;
    }
}
