using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossStageEntryUI : PopupBase
{
    [SerializeField, Tooltip("Boss List View")]
    ScrollRect Boss_List_View;

    [SerializeField, Tooltip("Entrance Count Title")]
    TMP_Text Entrance_Count_Title;
    [SerializeField, Tooltip("Entrance Count")]
    TMP_Text Entrance_Count;

    List<BossStageEntrySlot> Used_Boss_Slot_List = new List<BossStageEntrySlot>();


    protected override bool Initialize(object[] data)
    {
        InitAssets();
        
        return true;
    }

    void InitAssets()
    {
        List<string> asset_list = new List<string>();

        //  boss slot prefab
        var m = MasterDataManager.Instance;
        var boss_mng = GameData.Instance.GetUserBossStageDataManager();

        //  보스 리스트
        var dungeon = boss_mng.GetDungeonData();
        var boss_list = m.Get_BossDataList(dungeon.dungeon_group_id);
        for (int i = 0; i < boss_list.Count; i++)
        {
            var boss = boss_list[i];
            if (!asset_list.Contains(boss.prefab_path))
            {
                asset_list.Add(boss.prefab_path);
            }
        }

        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(asset_list, PreloadCallback);
    }
    void PreloadCallback(int load_cnt, int total_cnt)
    {
        if (load_cnt >= total_cnt)
        {
            FixedUpdatePopup();
        }
    }

    protected override void FixedUpdatePopup()
    {
        ClearBossSlots();

        var m = MasterDataManager.Instance;
        var boss_mng = GameData.Instance.GetUserBossStageDataManager();

        //  보스 리스트
        var dungeon = boss_mng.GetDungeonData();
        var boss_list = m.Get_BossDataList(dungeon.dungeon_group_id);
        var pool = GameObjectPoolManager.Instance;
        for (int i = 0; i < boss_list.Count; i++)
        {
            var boss = boss_list[i];
            var obj = pool.GetGameObject(boss.prefab_path, Boss_List_View.content);
            var slot = obj.GetComponent<BossStageEntrySlot>();
            slot.SetBossDataID(boss.boss_id);
            Used_Boss_Slot_List.Add(slot);
        }

        //  입장 제한 횟수
        int entrance_cnt = boss_mng.GetEntranceCount();
        int max_entrance_cnt = boss_mng.GetMaxEntranceCount();
        Entrance_Count.text = ZString.Format("{0}/{1}", entrance_cnt, max_entrance_cnt);
    }

    void ClearBossSlots()
    {
        var pool = GameObjectPoolManager.Instance;
        for (int i = 0; i < Used_Boss_Slot_List.Count; i++)
        {
            pool.UnusedGameObject(Used_Boss_Slot_List[i].gameObject);
        }
        Used_Boss_Slot_List.Clear();
    }
}
