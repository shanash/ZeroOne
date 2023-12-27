using UnityEngine;

public class StageBase : MonoBehaviour
{
    public Transform Background_Parent
    {
        get => transform;
    }

    public Transform Actor_Parent
    {
        get => transform.Find("Actress_Container");
    }

    bool Initialize()
    {
        return true;
    }
}
