using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetArrowNode : MonoBehaviour, IPoolableComponent
{
    Transform Target_Transform;

   
    public void SetTargetTransform(Transform target)
    {
        Target_Transform = target;
    }


    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (Target_Transform != null)
        {
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, Target_Transform.position);
            this.transform.position = pos;
        }
    }

    public void Spawned()
    {
    }

    public void Despawned()
    {
        Target_Transform = null;
    }

}
