
using LitJson;

public class UserMemorialData : UserDataBase
{
    public int Memorial_ID { get; protected set; } = 0;

    public int Player_Character_ID { get; protected set; } = 0;

    public int Lobby_Choice_Number { get; protected set; } = 0;

    public bool Is_Choice_Lobby { get; protected set; }  = false;

    /// <summary>
    /// 임시 지정번호
    /// </summary>
    public int Temp_Lobby_Choice_Number { get; protected set; } = 0;
    /// <summary>
    /// 임시 선택
    /// </summary>
    public bool Is_Temp_Choice { get; protected set; } = false;

    Me_Resource_Data Data;

    public UserMemorialData() : base() { }

    protected override void Reset()
    {
        Memorial_ID = 0;
        Player_Character_ID = 0;
        Lobby_Choice_Number = 0;
        Is_Choice_Lobby = false;
        Temp_Lobby_Choice_Number = 0;
        Is_Temp_Choice = false;
    }

    public void SetMemorialDataID(int memorial_id, int player_character_id)
    {
        Memorial_ID = memorial_id;
        Player_Character_ID = player_character_id;
        InitMasterData();
    }

    protected override void InitMasterData()
    {
        var m = MasterDataManager.Instance;
        Data = m.Get_MemorialResourceData(Memorial_ID, Player_Character_ID);
    }

    public Me_Resource_Data GetMemorialData()
    {
        return Data;
    }

    public void SetTempLobbyChoiceNumber(int num)
    {
        Temp_Lobby_Choice_Number = num;
        Is_Temp_Choice = Temp_Lobby_Choice_Number > 0;
    }

    public void ResetTempLobbyChoice()
    {
        SetTempLobbyChoiceNumber(0);
    }

    public void SetLobbyChoiceNumber(int num)
    {
        Lobby_Choice_Number = num;
        Is_Choice_Lobby = Lobby_Choice_Number > 0;
    }
    public void ReleaseLobbyChoice()
    {
        SetLobbyChoiceNumber(0);
    }



    public override JsonData Serialized()
    {
        var json = new LitJson.JsonData();
        json[NODE_MEMORIAL_ID] = Memorial_ID;
        json[NODE_PLAYER_CHARACTER_ID] = Player_Character_ID;
        json[NODE_LOBBY_CHOICE_NUMBER] = Lobby_Choice_Number;
        json[NODE_IS_LOBBY_CHOICE] = Is_Choice_Lobby;

        return json;
    }

    public override bool Deserialized(JsonData json)
    {
        if (json == null)
        {
            return false;
        }
        if (json.ContainsKey(NODE_MEMORIAL_ID))
        {
            Memorial_ID = ParseInt(json, NODE_MEMORIAL_ID);
        }
        if (json.ContainsKey(NODE_PLAYER_CHARACTER_ID))
        {
            Player_Character_ID = ParseInt(json, NODE_PLAYER_CHARACTER_ID);
        }
        if (json.ContainsKey(NODE_LOBBY_CHOICE_NUMBER))
        {
            Lobby_Choice_Number = ParseInt(json, NODE_LOBBY_CHOICE_NUMBER);
        }
        if (json.ContainsKey(NODE_IS_LOBBY_CHOICE))
        {
            Is_Choice_Lobby = ParseBool(json, NODE_IS_LOBBY_CHOICE);
        }

        InitMasterData();
        return true;
    }

    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_MEMORIAL_ID = "mid";
    protected const string NODE_PLAYER_CHARACTER_ID = "pid";
    protected const string NODE_LOBBY_CHOICE_NUMBER = "cnum";
    protected const string NODE_IS_LOBBY_CHOICE = "choice";
}
