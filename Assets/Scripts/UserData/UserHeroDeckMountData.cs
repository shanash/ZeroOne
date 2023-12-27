

using LitJson;

public class UserHeroDeckMountData : UserDataBase
{
    public int Player_Character_ID { get; protected set; } = 0;

    public int Player_Character_Num { get; protected set; } = 0;

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
        Player_Character_ID = 0;
        Player_Character_Num = 0;
        Is_Leader = false;
        User_Hero_Data = null;
    }

    public UserHeroData GetUserHeroData()
    {
        return User_Hero_Data;
    }

    public UserHeroDeckMountData SetUserHeroData(int player_character_id, int player_character_num)
    {
        Player_Character_ID = player_character_id;
        Player_Character_Num = player_character_num;

        InitMasterData();
        return this;
    }

    protected override void InitMasterData()
    {
        var hero_mng = GameData.Instance.GetUserHeroDataManager();
        User_Hero_Data = hero_mng.FindUserHeroData(Player_Character_ID, Player_Character_Num);
        Is_Update_Data = true;
    }

    public void SetLeader(bool leader)
    {
        if (Is_Leader != leader)
        {
            Is_Update_Data = true;
        }
        Is_Leader = leader;
    }

    public override JsonData Serialized()
    {
        var json = new JsonData();
        json[NODE_PLAYER_CHARACTER_ID] = Player_Character_ID;
        json[NODE_PLAYER_CHARACTER_NUM] = Player_Character_Num;
        json[NODE_IS_LEADER] = Is_Leader;
        return json;
    }
    public override bool Deserialized(JsonData json)
    {
        if (json == null)
        {
            return false;
        }

        if (json.ContainsKey(NODE_PLAYER_CHARACTER_ID))
        {
            Player_Character_ID = ParseInt(json, NODE_PLAYER_CHARACTER_ID);
        }
        if (json.ContainsKey(NODE_PLAYER_CHARACTER_NUM))
        {
            Player_Character_Num = ParseInt(json, NODE_PLAYER_CHARACTER_NUM);
        }
        if (json.ContainsKey(NODE_IS_LEADER))
        {
            Is_Leader = ParseBool(json, NODE_IS_LEADER);
        }

        InitMasterData();

        return true;
    }

    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_PLAYER_CHARACTER_ID = "pid";
    protected const string NODE_PLAYER_CHARACTER_NUM = "pnum";
    protected const string NODE_IS_LEADER = "leader";

}
