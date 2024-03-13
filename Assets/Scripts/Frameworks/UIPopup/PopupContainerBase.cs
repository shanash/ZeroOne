using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyDuck.UI
{
    public abstract class PopupContainerBase : MonoBehaviour, IPoolableComponent
    {
        Dictionary<POPUP_TYPE, RectTransform> _Containers = null;

        public Dictionary<POPUP_TYPE, RectTransform> Get
        {
            get
            {
                if (_Containers == null)
                {
                    InitializeContainer(ref _Containers);
                }
                return _Containers;
            }
        }

        protected abstract void InitializeContainer(ref Dictionary<POPUP_TYPE, RectTransform> container);

        public abstract void SetUICamera();
        public abstract void Despawned();
        public abstract void Spawned();
    }
}
