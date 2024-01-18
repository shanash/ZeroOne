
/// <summary>
/// 캐릭터 및 캐릭터 조각 아이템
/// </summary>
public class ItemData_CharacterData_V2 : ItemDataBase_V2
{
    Player_Character_Data Data;

    public override void SetItem(ITEM_TYPE_V2 gtype, int item_id)
    {
        base.SetItem(gtype, item_id);
        Data = MasterDataManager.Instance.Get_PlayerCharacterData(item_id);
    }

    public override object GetItemData()
    {
        return Data;
    }
    public override string GetItemIconPath()
    {
        if (Data != null)
        {
            return Data.icon_path;
        }
        return string.Empty;
    }


}
