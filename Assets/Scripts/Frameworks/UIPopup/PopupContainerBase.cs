using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;
using FluffyDuck.UI;

public abstract class PopupContainerBase : MonoBehaviour, IPoolableComponent
{
    public abstract Dictionary<POPUP_TYPE, RectTransform> Get { get; }

    public abstract void SetUICamera();

    public abstract void Despawned();
    public abstract void Spawned();
}
