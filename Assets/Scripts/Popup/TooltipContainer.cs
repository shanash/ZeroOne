using FluffyDuck.UI;
using System.Collections.Generic;
using UnityEngine;

public class TooltipContainer : PopupContainerBase
{
    [SerializeField, Tooltip("툴팁 컨테이너")]
    RectTransform _Tooltip_Container = null;

    protected override void InitializeContainer(ref Dictionary<POPUP_TYPE, RectTransform> container)
    {
        container = new Dictionary<POPUP_TYPE, RectTransform>()
        {
            [POPUP_TYPE.FULLPAGE_TYPE] = _Tooltip_Container,
        };
    }

    public override void Despawned()
    {
    }

    public override void SetUICamera()
    {
    }

    public override void Spawned()
    {
    }
}
