using System.Collections.Generic;
using UnityEngine;

public class EffectHideComponent : MonoBehaviour
{
    [SerializeField, Tooltip("Hide Transforms")]
    List<Transform> Hide_Transforms;

    [SerializeField, Tooltip("Delay Time")]
    float Delay_Time;

    public void Show(bool show)
    {
        Hide_Transforms.ForEach(x => x.gameObject.SetActive(show));
    }

    public float GetDelayTime()
    {
        return Delay_Time;
    }
}
