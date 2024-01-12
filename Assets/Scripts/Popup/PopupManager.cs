using FluffyDuck.UI;
using FluffyDuck.Util;
using UnityEngine;

public class PopupManager : PopupManagerBase<PopupManager>
{
    PopupContainer _Container;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void CallInstance()
    {
        _ = Instance;
    }

    protected override PopupContainer Container
    {
        get
        {
            if (_Container == null)
            {
                InitContainer();
            }

            return _Container;
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        InitContainer();
    }

    void InitContainer()
    {
        var go = GameObjectPoolManager.Instance.GetGameObject("Prefabs/Popup_Container", this.transform);
        _Container = go.GetComponent<PopupContainer>();
    }
}
