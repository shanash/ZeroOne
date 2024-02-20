using LitJson;
using System.Diagnostics;

public class UserL2dData : UserDataBase
{
    public int Skin_Id { get; protected set; } = 0;

    public int Lobby_Choice_Number { get; protected set; } = 0;

    public bool Is_Choice_Lobby { get; protected set; } = false;

    public int Player_Character_ID { get; protected set; } = 0;


    L2d_Char_Skin_Data Data;

    public UserL2dData() : base() { }

    protected override void Reset()
    {
        Skin_Id = 0;
        Lobby_Choice_Number = 0;
        Is_Choice_Lobby = false;
    }

    public void SetL2dDataID(int memorial_id, int hero_id)
    {
        Skin_Id = memorial_id;
        Player_Character_ID = hero_id;
        InitMasterData();
    }

    protected override void InitMasterData()
    {
        var m = MasterDataManager.Instance;
        Data = m.Get_L2DCharSkinData(Skin_Id);
    }

    public L2d_Char_Skin_Data GetL2dData()
    {
        return Data;
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

    public override object Clone()
    {
        UserL2dData clone = (UserL2dData)this.MemberwiseClone();
        clone.Data = this.Data;

        return clone;
    }

    public override JsonData Serialized()
    {
        //if (!IsUpdateData())
        //{
        //    return null;
        //}
        var json = new LitJson.JsonData();
        json[NODE_MEMORIAL_ID] = Skin_Id;
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
            Skin_Id = ParseInt(json, NODE_MEMORIAL_ID);
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
    protected const string NODE_LOBBY_CHOICE_NUMBER = "cnum";
    protected const string NODE_IS_LOBBY_CHOICE = "choice";
}
