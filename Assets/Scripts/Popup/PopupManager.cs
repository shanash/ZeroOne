using FluffyDuck.UI;
using FluffyDuck.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupManager : PopupManagerBase<PopupManager>
{
    PopupContainer _Container = null;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void CallInstance()
    {
        _ = Instance;
    }

    public override PopupContainerBase Container
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
        string scene_name = SceneManager.GetActiveScene().name;
        if (scene_name.Equals("battle"))
        {
            var go = GameObjectPoolManager.Instance.GetGameObject("Prefabs/Battle_Popup_Container", this.transform);
            _Container = go.GetComponent<PopupContainer>();
            _Container.SetUICamera();
        }
        else
        {
            var go = GameObjectPoolManager.Instance.GetGameObject("Prefabs/Popup_Container", this.transform);
            _Container = go.GetComponent<PopupContainer>();
        }
    }
}
