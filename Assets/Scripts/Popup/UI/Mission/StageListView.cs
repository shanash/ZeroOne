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
        var m = MasterDataManager.Instance;
        World = m.Get_WorldData(World_ID);
        Zone = m.Get_ZoneData(Zone_ID);
        RefreshScrollView();
    }

    void RefreshScrollView()
    {
        ClearListCells();
        var mng = GameData.Instance.GetUserStoryStageDataManager();

    }

    void ClearListCells()
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
