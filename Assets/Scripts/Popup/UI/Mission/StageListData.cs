using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageListData : InfiniteScrollData
{
    public UserStoryStageData User_Data;
    public Stage_Data Data;


    public void SetStageID(int stage_id)
    {
        Data = MasterDataManager.Instance.Get_StageData(stage_id);

        var mng = GameData.Instance.GetUserStoryStageDataManager();
        User_Data = mng.FindUserStoryStageData(stage_id);
    }
    

}
