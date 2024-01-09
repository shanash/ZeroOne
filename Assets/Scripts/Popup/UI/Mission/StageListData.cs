using Gpm.Ui;

public class StageListData : InfiniteScrollData
{
    public UserStoryStageData User_Data;
    public Stage_Data Data;
    public Zone_Data Zone;


    public void SetStageID(int stage_id)
    {
        var m = MasterDataManager.Instance;
        Data = m.Get_StageData(stage_id);

        Zone = m.Get_ZoneData(Data.zone_id);

        var mng = GameData.Instance.GetUserStoryStageDataManager();
        User_Data = mng.FindUserStoryStageData(stage_id);
    }

    public bool IsExistUserData()
    {
        return User_Data != null;
    }


}
