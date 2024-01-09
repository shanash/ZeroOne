using FluffyDuck.Util;
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
