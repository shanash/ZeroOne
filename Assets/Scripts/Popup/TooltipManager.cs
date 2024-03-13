using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TooltipManager : PopupManagerBase<TooltipManager>
{
    TooltipContainer _Container = null;

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
                Initialize();
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
        var go = GameObjectPoolManager.Instance.GetGameObject("Prefabs/Tooltip_Container", this.transform);
        _Container = go.GetComponent<TooltipContainer>();

        /*
        string scene_name = SceneManager.GetActiveScene().name;
        if (scene_name.Equals("battle"))
        {
            var go = GameObjectPoolManager.Instance.GetGameObject("Prefabs/Battle_Popup_Container", this.transform);
            _Container = go.GetComponent<PopupContainer>();
            _Container.SetUICamera();
        }
        else
        {
            var go = GameObjectPoolManager.Instance.GetGameObject("Prefabs/Tooltip_Container", this.transform);
            _Container = go.GetComponent<PopupContainer>();
        }
        */
    }

    /// <summary>
    /// 툴팁에서는 이쪽을 사용하기
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cb"></param>
    public void Add(string path, System.Action<PopupBase> cb, Rect rect, params object[] datas)
    {
        Add(path, POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
        {
            popup.ShowPopup(rect, datas);
            cb(popup);
        });
    }

    public void Add(string path, Rect rect, params object[] datas)
    {
        Add(path, POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
        {
            if (popup is not TooltipBase)
            {
                Debug.Assert(false, "호출한 Prefab이 툴팁이 아닙니다!");
                Destroy(popup.gameObject);
                return;
            }

            var tooltip = popup as TooltipBase;

            // 새로운 object[] 배열을 생성, 첫 번째 위치에 rect를 넣고, 나머지 위치에 datas의 요소들을 넣음
            object[] arguments = new object[datas.Length + 1];
            arguments[0] = rect;
            datas.CopyTo(arguments, 1);

            // 수정된 arguments 배열을 tooltip.ShowPopup에 전달
            tooltip.ShowPopup(arguments);
        });
    }
}
