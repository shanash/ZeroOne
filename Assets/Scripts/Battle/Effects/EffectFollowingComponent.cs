using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFollowingComponent : MonoBehaviour
{
    [SerializeField, Tooltip("Target")]
    Transform Target;

    public void SetTarget(Transform t)
    {
        Target = t;
    }

    private void Update()
    {
        if (Target == null) { return; }

        var pos = Target.position;
        var this_pos = this.transform.position;
        pos.z = this_pos.z;
        this.transform.position = pos;
    }

    public void ResetComponent()
    {
        Target = null;
    }
}
