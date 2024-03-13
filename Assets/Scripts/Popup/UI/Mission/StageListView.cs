using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageListView : MonoBehaviour
{
    [SerializeField, Tooltip("난이도 타입")]
    STAGE_DIFFICULTY_TYPE Difficulty_Type = STAGE_DIFFICULTY_TYPE.NONE;

    [SerializeField, Tooltip("Scroll View")]
    ScrollRect Scroll_View;

    List<StageListCell> Used_Stage_List_Cell = new List<StageListCell>();

    int World_ID;
    int Zone_ID;
    Zone_Data Zone;
    World_Data World;

    public void SetWorldAndZoneID(int world_id, int zone_id)
    {
        World_ID = world_id;
        Zone_ID = zone_id;
        if (Zone_ID == 0)
        {
            ClearListCells();
            return;
        }
        var m = MasterDataManager.Instance;
        World = m.Get_WorldData(World_ID);
        Zone = m.Get_ZoneData(Zone_ID);
        RefreshScrollView();
    }

    void RefreshScrollView()
    {
        ClearListCells();
        var m = MasterDataManager.Instance;
        var stage_list_data = m.Get_StageDataListByStageGroupID(Zone.stage_group_id);
        var pool = GameObjectPoolManager.Instance;
        string prefab_name = string.Empty;
        switch (Difficulty_Type)
        {
            case STAGE_DIFFICULTY_TYPE.NORMAL:
                prefab_name = "Assets/AssetResources/Prefabs/Popup/UI/Mission/NormalStageListCell";
                break;
            case STAGE_DIFFICULTY_TYPE.HARD:
                prefab_name = "Assets/AssetResources/Prefabs/Popup/UI/Mission/HardStageListCell";
                break;
            case STAGE_DIFFICULTY_TYPE.VERY_HARD:
                prefab_name = "Assets/AssetResources/Prefabs/Popup/UI/Mission/HardStageListCell";
                break;
            default:
                prefab_name = "Assets/AssetResources/Prefabs/Popup/UI/Mission/NormalStageListCell";
                break;
        }

        for (int i = 0; i < stage_list_data.Count; i++)
        {
            var obj = pool.GetGameObject(prefab_name, Scroll_View.content);
            var cell = obj.GetComponent<StageListCell>();
            cell.SetStageData(stage_list_data[i]);
            Used_Stage_List_Cell.Add(cell);
        }
    }

    public void ClearListCells()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Stage_List_Cell.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Stage_List_Cell[i].gameObject);
        }
        Used_Stage_List_Cell.Clear();
    }
    public STAGE_DIFFICULTY_TYPE GetDifficultyType()
    {
        return Difficulty_Type;
    }
}
