using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScalePopup : PopupBase
{

    [SerializeField, Tooltip("Content View")]
    RectTransform Content_View;

    List<TestListNode> Used_List_Nodes = new List<TestListNode>();

    string Prefab_Name = "Assets/AssetResources/Prefabs/Popup/Popup/TestListNode";

    public override void ShowPopup(params object[] data)
    {
        base.ShowPopup(data);

        List<string> list = new List<string>();
        list.Add(Prefab_Name);
        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(list, null);
    }

    protected override void ShowPopupAniEndCallback()
    {
        FixedUpdatePopup();
    }

    protected override void FixedUpdatePopup()
    {
        var pool = GameObjectPoolManager.Instance;
        int count = 10;
        for (int i = 0; i < count; i++)
        {
            var obj = pool.GetGameObject(Prefab_Name, Content_View);
            var node = obj.GetComponent<TestListNode>();
            node.SetListText((i+1).ToString());
            Used_List_Nodes.Add(node);
        }
    }

    public override void HidePopup(Action cb = null)
    {
        base.HidePopup(cb);
        ClearListNodes();
    }

    void ClearListNodes()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_List_Nodes.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_List_Nodes[i].gameObject);
        }
        Used_List_Nodes.Clear();
    }
}
