using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour, IPoolableComponent
{
    public virtual void Despawned()
    {
    }

    public virtual void Spawned()
    {
    }
}
