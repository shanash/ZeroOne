

public class UserHeroDeckMountData : UserDataBase
{
    public int Hero_Data_ID { get; protected set; } = 0;

    public int Hero_Data_Num { get; protected set; } = 0;

    public bool Is_Leader { get; protected set; } = false;

    UserHeroData User_Hero_Data;
    

    public UserHeroDeckMountData()
        : base()
    {

    }

    protected override void Reset()
    {
        
    }

    protected override void Destroy()
    {
        Hero_Data_ID = 0;
        Hero_Data_Num = 0;
        Is_Leader = false;
        User_Hero_Data = null;
    }

    public UserHeroData GetUserHeroData()
    {
        return User_Hero_Data;
    }

    public UserHeroDeckMountData SetUserHeroData(UserHeroData hero)
    {
        Hero_Data_ID = hero.GetPlayerCharacterID();
        Hero_Data_Num = hero.Player_Character_Num;
        Is_Leader = false;
        User_Hero_Data = hero;
        return this;
    }

    public UserHeroDeckMountData SetUserHeroData(int hero_data_id, int hero_data_num, bool leader)
    {
        Hero_Data_ID = hero_data_id;
        Hero_Data_Num = hero_data_num;
        Is_Leader = leader;

        var hero_mng = GameData.Instance.GetUserHeroDataManager();
        User_Hero_Data = hero_mng.FindUserHeroData(hero_data_id, hero_data_num);

        return this;
    }

    protected override void InitMasterData()
    {
        
    }

    public void SetLeader(bool leader)
    {
        if (Is_Leader != leader)
        {
            Is_Update_Data = true;
        }
        Is_Leader = leader;
    }
}
