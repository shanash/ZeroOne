using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossStageChoiceUI : PopupBase
{
    [Header("Left Box")]
    [SerializeField, Tooltip("Blur")]
    Image Back_Blur;
    [SerializeField, Tooltip("Boss Image")]
    Image Boss_Image;

    [SerializeField, Tooltip("Boss Nick")]
    TMP_Text Boss_Nick;
    [SerializeField, Tooltip("Boss Name")]
    TMP_Text Boss_Name;

    [SerializeField, Tooltip("Extra Rank Title")]
    TMP_Text Extra_Rank_Title;
    [SerializeField, Tooltip("Extra Stage Btn")]
    UIButtonBase Extra_Stage_Btn;
    [SerializeField, Tooltip("Extra Stage Lock")]
    Image Extra_Stage_Lock;

    [SerializeField, Tooltip("Boss Info List View")]
    ScrollRect Boss_Info_List_View;

    [SerializeField, Tooltip("Boss Info Text")]
    TMP_Text Boss_Info_Text;

    [Space()]
    [Header("Right Box")]
    [SerializeField, Tooltip("Entrance Count")]
    TMP_Text Entrance_Count;

    [SerializeField, Tooltip("Stage List View")]
    ScrollRect Stage_List_View;

    List<BossStageListNode> Used_Boss_Stage_List_Nodes = new List<BossStageListNode>();

    Boss_Data Boss;


    protected override bool Initialize(object[] data)
    {
        if (data.Length != 1)
        {
            return false;
        }

        Boss = (Boss_Data)data[0];

        InitAssets();
        return true;
    }

    void InitAssets()
    {
        List<string> asset_list = new List<string>();
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/UI/BossDungeon/BossStageListNode");

        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(asset_list, PreloadCallback);

    }
    void PreloadCallback(int load_cnt, int total_cnt)
    {
        if (load_cnt >= total_cnt)
        {
            FixedUpdatePopup();
            UpdatePopup();
        }
    }

    protected override void FixedUpdatePopup()
    {
        ClearBossStageListNode();
        //  left box
        var sb = ZString.CreateStringBuilder();
        sb.AppendLine(GameDefine.GetLocalizeString(Boss.boss_story_info));
        sb.AppendLine();
        sb.AppendLine(GameDefine.GetLocalizeString(Boss.boss_skill_info));
        Boss_Info_Text.text = sb.ToString();

        //  right box
        var m = MasterDataManager.Instance;
        var pool = GameObjectPoolManager.Instance;
        var boss_stage_list = m.Get_BossStageDataListByBossStageGroupID(Boss.boss_stage_group_id);
        int cnt = boss_stage_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var stage = boss_stage_list[i];
            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/Popup/UI/BossDungeon/BossStageListNode", Stage_List_View.content);
            var node = obj.GetComponent<BossStageListNode>();
            node.SetBossStageData(stage);
            node.SetSkipRefreshCallback(UpdatePopup);
            Used_Boss_Stage_List_Nodes.Add(node);
        }
    }

    public override void UpdatePopup()
    {
        //  일일 입장 제한
        var boss_mng = GameData.Instance.GetUserBossStageDataManager();
        int entrance_cnt = boss_mng.GetEntranceCount();
        int max_entrance_cnt = boss_mng.GetMaxEntranceCount();
        Entrance_Count.text = ZString.Format("{0} {1}/{2}", GameDefine.GetLocalizeString("system_limitcount_daily_enter"), entrance_cnt, max_entrance_cnt);
    }

    void ClearBossStageListNode()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Boss_Stage_List_Nodes.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Boss_Stage_List_Nodes[i].gameObject);
        }
        Used_Boss_Stage_List_Nodes.Clear();
    }

    public override void Despawned()
    {
        base.Despawned();

        Boss_Info_Text.text = string.Empty;
        Entrance_Count.text = string.Empty;
    }
}
