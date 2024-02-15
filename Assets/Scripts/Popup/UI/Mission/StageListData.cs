using Gpm.Ui;

public class StageListData : InfiniteScrollData
{
    public UserStoryStageData User_Data;
    public Stage_Data Stage;

    public World_Data World;
    public Zone_Data Zone;


    public void SetStageID(int world_id, int zone_id, int stage_id)
    {
        var m = MasterDataManager.Instance;
        Stage = m.Get_StageData(stage_id);
        World = m.Get_WorldData(world_id);
        Zone = m.Get_ZoneData(zone_id);

        var mng = GameData.Instance.GetUserStoryStageDataManager();
        User_Data = mng.FindUserStoryStageData(stage_id);
    }

    public bool IsExistUserData()
    {
        return User_Data != null;
    }


}
